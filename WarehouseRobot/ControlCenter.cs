using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    public class ControlCenter
    {
        private Grid grid = new();
        public List<Robot> robots = new();
        public Dictionary<Robot, TransportTask> runningTasks = new();
        public ControlCenter()
        {
            for (int i = 1; i <= 30; ++i)
            {
                robots.Add(new Robot());
            }
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
        public Stack<(uint, uint)> CalcPath(Robot robot, TransportTask task)
        {
            Stack<(uint, uint)> route = new();
            (uint, uint) from = task.from;
            (uint, uint) to = task.to;

            route.Push(from);
            while (from.Item1 != to.Item1)
            {
                if (from.Item1 < to.Item1)
                {
                    ++from.Item1;
                }
                else
                {
                    --from.Item1;
                }
                route.Push(from);
            }
            while (from.Item2 != to.Item2)
            {
                if (from.Item2 < to.Item2)
                {
                    ++from.Item2;
                }
                else
                {
                    --from.Item2;
                }
                route.Push(from);
            }
            return new Stack<(uint, uint)>(route.ToArray());
        }
        /// <summary>
        /// 检测所有机器人的路线冲突
        /// </summary>
        /// <param name="forsee">向前看几步</param>
        public List<RouteConflictInfo> DetectConflict(int forsee = 5)
        {
            List<RouteConflictInfo> conflictInfos = new();
            List<Robot> robotsToDetect = new();
            List<List<(uint, uint)>> routes = new();
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in runningTasks)
            {
                robotsToDetect.Add(keyValuePair.Key);
                routes.Add(keyValuePair.Key.Route.ToList());
            }

            Dictionary<(uint, uint), Robot> conflictDict = new();
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
        public List<(uint,uint)> GetCurrentCollision()
        {
            List<(uint, uint)> conflictPosition = new();
            HashSet<(uint, uint)> conflict = new();
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
            uint maxRow = grid.Size.Item1;
            uint maxCol = grid.Size.Item2;
            for (uint i = 0; i < maxRow; ++i)
            {
                for (uint j = 0; j < maxCol; ++j)
                {
                    Console.SetCursorPosition((int)j * 2, (int)i);
                    if (grid.GetPositionState(i, j) == Enum.ZoneState.Blocked)
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
                (uint, uint) pos = keyValuePair.Key.CurrentPosition;
                Console.SetCursorPosition((int)pos.Item2 * 2, (int)pos.Item1);
                Console.Write("^ ");
            }

            List<RouteConflictInfo> conflicts = DetectConflict();
            foreach (RouteConflictInfo conflict in conflicts)
            {
                (uint, uint) pos1 = conflict.Robot1.CurrentPosition;
                (uint, uint) pos2 = conflict.Robot2.CurrentPosition;
                (uint, uint) pos = conflict.Where;

                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition((int)pos1.Item2 * 2, (int)pos1.Item1);
                Console.Write("^ ");
                Console.SetCursorPosition((int)pos2.Item2 * 2, (int)pos2.Item1);
                Console.Write("^ ");
                Console.BackgroundColor = ConsoleColor.Black;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((int)pos.Item2 * 2, (int)pos.Item1);
                Console.Write("! ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            foreach((uint,uint) position in GetCurrentCollision())
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition((int)position.Item2 * 2, (int)position.Item1);
                Console.Write("X ");
                Console.ResetColor();
            }
        }
    }
}
