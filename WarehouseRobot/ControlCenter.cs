using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;
using WarehouseRobot.Utility;

namespace WarehouseRobot
{
    public class ControlCenter
    {
        public event Action OnOneTaskFinished;

        private AStarRoutePlanner aStarRoutePlanner = null;
        /// <summary>
        /// 所有的机器人
        /// </summary>
        public List<Robot> Robots
        {
            get;
            private set;
        } = new();
        /// <summary>
        /// 正在运行的任务
        /// </summary>
        public Dictionary<Robot, TransportTask> RunningTasks
        {
            get;
            private set;
        } = new();
        /// <summary>
        /// 机器人数量
        /// </summary>
        public int RobotNum
        {
            get;
        } = 5;
        /// <summary>
        /// 排队任务数
        /// </summary>
        public int QueuingTaskCount
        {
            get { return waitingTask.Count; }
        }

        private Queue<TransportTask> waitingTask = new();
        private ZoneState[,] grid = null;
        public (int, int) Size
        {
            get
            {
                return (grid.GetLength(0), grid.GetLength(1));
            }
        }

        public ControlCenter(int rowCount = 30, int colCount = 30)
        {
            for (int i = 1; i <= RobotNum; ++i)
            {
                Robots.Add(new Robot());
            }
            grid = GridGenerator.GetGrid(rowCount, colCount);
            aStarRoutePlanner = new AStarRoutePlanner(grid, new SimpleCostGetter());
        }
        /// <summary>
        /// 计算下一个时间点时机器人的信息
        /// </summary>
        public void NextTick()
        {
            foreach (Robot r in Robots)
            {
                if (r.State == RobotState.Finished)
                {
                    RunningTasks.Remove(r);
                    r.Reset();
                    if (waitingTask.Count > 0)
                        AssignTask(waitingTask.Dequeue());
                    // 调用事件
                    OnOneTaskFinished();
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
            Robot idleRobot = FindIdleRobot();
            if (idleRobot == null)
            {
                waitingTask.Enqueue(task);
            }
            else
            {
                RunningTasks.Add(idleRobot, task);
                idleRobot.SetTask(CalcPath(idleRobot, task));
            }
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
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in RunningTasks)
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
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in RunningTasks)
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
            int maxRow = Size.Item1;
            int maxCol = Size.Item2;
            for (uint i = 0; i < maxRow; ++i)
            {
                for (uint j = 0; j < maxCol; ++j)
                {
                    Console.SetCursorPosition((int)j * 2, (int)i);
                    if (grid[i, j] == ZoneState.Blocked)
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

            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in RunningTasks)
            {
                Point pos = keyValuePair.Key.CurrentPosition;
                Console.SetCursorPosition(pos.X * 2, pos.Y);
                Console.Write("^ ");
            }

            List<RouteConflictInfo> conflicts = DetectConflict();
            foreach (RouteConflictInfo conflict in conflicts)
            {
                Point pos1 = conflict.Robot1.CurrentPosition;
                Point pos2 = conflict.Robot2.CurrentPosition;
                Point pos = conflict.Where;


                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pos1.X * 2, pos1.Y);
                Console.Write("^ ");
                Console.SetCursorPosition(pos2.X * 2, pos2.Y);
                Console.Write("^ ");
                Console.BackgroundColor = ConsoleColor.Black;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(pos.X * 2, pos.Y);
                Console.Write("! ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            foreach (Point position in GetCurrentCollision())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(position.X * 2, position.Y);
                Console.Write("X ");
                Console.ResetColor();
            }
        }
        public void Print(Graphics g)
        {
            Brush brush = new SolidBrush(Color.White);
            for (uint i = 0; i < Size.Item1; ++i)
            {
                for (uint j = 0; j < Size.Item2; ++j)
                {
                    g.FillRectangle(brush, j * 20 + 1, i * 20 + 1, 19, 19);
                }
            }

            brush = new SolidBrush(Color.Black);
            for (uint i = 0; i < Size.Item1; ++i)
            {
                for (uint j = 0; j < Size.Item2; ++j)
                {
                    if (grid[i, j] == ZoneState.Blocked)
                        g.FillRectangle(brush, j * 20 + 1, i * 20 + 1, 19, 19);
                }
            }

            brush = new SolidBrush(Color.Red);
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in RunningTasks)
            {
                Point pos = keyValuePair.Key.CurrentPosition;
                g.FillRectangle(brush, pos.X * 20 + 1, pos.Y * 20 + 1, 19, 19);
            }
        }

        private Robot FindIdleRobot() => Robots.Find(e => e.State == Enum.RobotState.Idle);
    }
}
