using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    public class ControlCenter
    {

        //private Grid grid = null;
        public List<Robot> robots = new();
        public Dictionary<Robot, TransportTask> runningTasks = new();
        AStarRoutePlanner aStarRoutePlanner = null;
        private static int ROBOTS_NUM = 5;//机器人数量
        private static int lineCount = 50;
        private static int columnCount = 30;
        ZoneState[][] grid = null;
        public (int,int) GetSize()
        {
            return (columnCount,lineCount);
        }
        public int GetRobotsNum()
        {
            return ROBOTS_NUM;
        }
        public ControlCenter()
        {
            for (int i = 1; i <= ROBOTS_NUM; ++i)
            {
                robots.Add(new Robot());
            }
            aStarRoutePlanner = new AStarRoutePlanner( columnCount, lineCount, new SimpleCostGetter());
            grid = aStarRoutePlanner.grid;
            // grid = new Grid(COL, ROW);
        }
        //public ControlCenter(Grid grid)
        //{
        //    this.grid = grid;
        //}
        /// <summary>
        /// 计算下一个时间点时机器人的信息
        /// </summary>
        public void NextTick()
        {
            foreach (Robot r in robots)
            {
                if (r.State == Enum.RobotState.Finished)
                {
                    runningTasks.Remove(r);
                    r.Reset();
                }
                r.Move();
            }
        }
        /// <summary>
        /// 为指挥中心分派任务
        /// </summary>
        /// <param name="task">需要被执行的任务</param>
        public void AssignTask(TransportTask task)
        {
            Robot idleRobot = robots.Find(e => e.State == Enum.RobotState.Idle);
            runningTasks.Add(idleRobot, task);
            idleRobot.SetTask(CalcPath(idleRobot, task));
        }
        /// <summary>
        /// 计算机器人的运输最短路径
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public IList<Point> CalcPath(Robot robot, TransportTask task)
        {
            IList<Point> route = aStarRoutePlanner.Plan(task.from, task.to);
            return route;
        }
        /// <summary>
        /// 检测所有机器人的路线冲突
        /// </summary>
        /// <param name="forsee">向前看几步</param>
        public List<RouteConflictInfo> DetectConflict(int forsee = 5)
        {
            List<RouteConflictInfo> conflictInfos = new();
            List<Robot> robotsToDetect = new();
            List<List<Point>> routes = new();
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in runningTasks)
            {
                robotsToDetect.Add(keyValuePair.Key);
                routes.Add(keyValuePair.Key.Route.ToList());
            }

            Dictionary<Point, Robot> conflictDict = new();
            List<int> robotToRemove = new();
            // 从现在往将来看i步，是否有冲突
            for (int i = 0; i < forsee; ++i)
            {
                // 检查第j个正在运行的机器人
                for (int j = 0; j < routes.Count; ++j)
                {
                    if (routes[j].Count > i)
                    {
                        if (conflictDict.ContainsKey(routes[j][i]))
                        {
                            // 检测到冲突，处理具体内容
                            conflictInfos.Add(new RouteConflictInfo(
                                conflictDict[routes[j][i]],
                                robotsToDetect[j],
                                routes[j][i],
                                i
                            ));
                        }
                        else
                        {
                            conflictDict.Add(routes[j][i], robotsToDetect[j]);
                        }
                    }
                    else
                    {
                        robotToRemove.Add(j);
                    }
                }

                // 务必排序，否则会越界
                robotToRemove.Sort((a, b) => b.CompareTo(a));
                foreach (int index in robotToRemove)
                {
                    robotsToDetect.RemoveAt(index);
                    routes.RemoveAt(index);
                }
                robotToRemove.Clear();
                conflictDict.Clear();
            }
            return conflictInfos;
        }
        /// <summary>
        /// 解决路线冲突的问题
        /// </summary>
        public void ResolveConflict()
        {
            throw new NotImplementedException();
        }
        public List<Point> GetCurrentCollision()
        {
            List<Point> conflictPosition = new();
            HashSet<Point> conflict = new();
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in runningTasks)
            {
                if (!conflict.Add(keyValuePair.Key.CurrentPosition))
                {
                    conflictPosition.Add(keyValuePair.Key.CurrentPosition);
                }
            }
            return conflictPosition;
        }
        public void Print()
        {
            
            Console.SetCursorPosition(0, 0);
            int maxRow = columnCount;
            int maxCol = lineCount;
            for (uint i = 0; i < maxRow; ++i)
            {
                for (uint j = 0; j < maxCol; ++j)
                {
                    Console.SetCursorPosition((int)j * 2, (int)i);
                    if (grid[i][j] == Enum.ZoneState.Blocked)
                    {

                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write("@");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in runningTasks)
            {
                Point pos = keyValuePair.Key.CurrentPosition;
                Console.SetCursorPosition((int)pos.Y * 2, (int)pos.X);
                Console.Write("^ ");
            }

            List<RouteConflictInfo> conflicts = DetectConflict();
            foreach (RouteConflictInfo conflict in conflicts)
            {
                Point pos1 = conflict.Robot1.CurrentPosition;
                Point pos2 = conflict.Robot2.CurrentPosition;
                Point pos = conflict.Where;
                

                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition((int)pos1.Y * 2, (int)pos1.X);
                Console.Write("^ ");
                Console.SetCursorPosition((int)pos2.Y * 2, (int)pos2.X);
                Console.Write("^ ");
                Console.BackgroundColor = ConsoleColor.Black;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((int)pos.Y * 2, (int)pos.X);
                Console.Write("! ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            foreach(Point position in GetCurrentCollision())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition((int)position.Y * 2, (int)position.X);
                Console.Write("X ");
                Console.ResetColor();
            }
        }
    }
}
