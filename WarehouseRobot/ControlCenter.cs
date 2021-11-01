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

        private Point beginPoint = new Point(0, 0);
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
        //public Dictionary<Robot, TransportTask> RunningTasks
        //{
        //    get;
        //    private set;
        //} = new();
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
                Robots.Add(new Robot()
                {
                    CurrentPosition = new Point(0, 0)
                });
            }
            grid = GridGenerator.GetGrid(out _, rowCount, colCount);
            aStarRoutePlanner = new AStarRoutePlanner(grid, new SimpleCostGetter());
        }

        public ControlCenter(ZoneState[,] grid)
        {
            for (int i = 1; i <= RobotNum; ++i)
            {
                Robots.Add(new Robot()
                {
                    CurrentPosition = new Point(0, 0)
                });
            }
            this.grid = grid;
            aStarRoutePlanner = new AStarRoutePlanner(grid, new SimpleCostGetter());
        }

        public void SetGird(ZoneState[,] grid)
        {
            this.grid = grid;
            aStarRoutePlanner = new AStarRoutePlanner(grid, new SimpleCostGetter());
        }
        /// <summary>
        /// 计算下一个时间点时机器人的信息
        /// </summary>
        public void NextTick()
        {
            foreach (Robot r in Robots)
            {
                if (r.State == RobotState.Oprating)
                {
                    if (r.Route.Count > 0)
                    {
                        // 还有路要走
                        r.State = RobotState.Running;
                    }
                    else
                    {
                        if (r.CurrentPosition == beginPoint)
                        {
                            // 运行结束，在起始位置
                            r.State = RobotState.Idle;
                            r.History.Clear();
                            r.Route?.Clear();
                        }
                        else
                        {
                            // 运行结束，不在起始位置，返回
                            r.State = RobotState.Returning;
                            r.Route = aStarRoutePlanner.Plan(r.CurrentPosition, beginPoint);
                        }
                    }
                    OnOneTaskFinished();
                }
                r.Move();
            }
            var confilcts = DetectConflict();
            SolveConflict(confilcts);
        }
        /// <summary>
        /// 为指挥中心分派任务
        /// </summary>
        /// <param name="task">需要被执行的任务</param>
        public void AssignTask(TransportTask task)
        {
            Robot robot = FindSuitableRobot(task.from);
            if (robot == null)
            {
                throw new NotImplementedException("不可能发生此情况。");
                //waitingTask.Enqueue(task);
            }
            else
            {
                if (robot.State == RobotState.Idle || robot.State == RobotState.Returning)
                {
                    TransportTask preTask = new()
                    {
                        from = robot.CurrentPosition,
                        to = task.from
                    };
                    IList<Point> suspendAt = new List<Point>()
                    {
                        task.from
                    };
                    IList<Point> totalTask = CalcPath(robot, preTask).Concat(CalcPath(robot, task)).ToList();
                    robot.SetTask(totalTask, suspendAt);
                }
                else if (robot.State == RobotState.Running)
                {
                    TransportTask preTask = new()
                    {
                        from = robot.Route.Last(),
                        to = task.from
                    };
                    IList<Point> suspendAt = new List<Point>()
                    {
                        robot.Route.Last(),
                        task.from
                    };
                    IList<Point> totalTask = robot.Route.Concat(CalcPath(robot, preTask)).Concat(CalcPath(robot, task)).ToList();
                    robot.SetTask(totalTask, suspendAt);
                }
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
        /// 更新即将冲突的机器人的路径
        /// </summary>
        /// <param name="conflictInfos">向前看几步</param>
        public void SolveConflict(List<RouteConflictInfo> conflictInfos)
        {
            foreach (RouteConflictInfo conflictInfo in conflictInfos)
            {
                if (conflictInfo.Step == 0)
                {
                    // 不要理会已经相撞的情况，这可能是由于两个机器人在同一时刻从同一点出发
                    continue;
                }
                ZoneState[,] tempGrid = (ZoneState[,])this.grid.Clone();
                // 将碰撞的地方设置为障碍物
                tempGrid[conflictInfo.Where.Y, conflictInfo.Where.X] = ZoneState.Blocked;
                // 传入新地图
                AStarRoutePlanner aStarRoutePlanner = new (tempGrid, new SimpleCostGetter());
                int step = conflictInfo.Step;
                // 从第i-1步开始重新规划路线
                IList<Point> newRoute = aStarRoutePlanner.Plan(conflictInfo.Robot2.Route[step - 1], conflictInfo.Robot2.Route.Last());
                if (newRoute == null || conflictInfo.Where == conflictInfo.Robot2.Route.Last() || conflictInfo.Robot2.SuspendAt.Contains(conflictInfo.Where))
                {
                    // 1.必须经过冲突点才能到达终点
                    // 2.冲突点就是终点
                    // 3.在冲突点装卸货
                    conflictInfo.Robot2.Route.Insert(conflictInfo.Step, conflictInfo.Robot2.Route[step - 1]); // 在碰撞前一个点等待;
                    return;
                }
                /* 将新的路径和之前的路径拼接起来 */
                for (int i = 0; i < conflictInfo.Step - 1; i++)
                {
                    newRoute.Insert(i, conflictInfo.Robot2.Route[i]);
                }

                if (TwoRobotHasConflict(conflictInfo.Robot1.Route, newRoute)) // 若重新规划路径之后还是冲突 就选择等待方案
                {
                    conflictInfo.Robot2.Route.Insert(conflictInfo.Step, conflictInfo.Robot2.Route[step - 1]); // 在碰撞前一个点等待;
                }
                else
                {
                    conflictInfo.Robot2.Route = newRoute;
                }
            }
        }

        /// <summary>
        /// 计算指定的两路径是否冲突
        /// </summary>
        /// <param name="route1"></param>
        /// <param name="route2"></param>
        /// <returns></returns>
        public bool TwoRobotHasConflict(IList<Point> route1, IList<Point> route2)
        {
            int length = route1.Count < route2.Count ? route1.Count : route2.Count;//获取两机器人路径长度的最小值
            for (int i = 0; i < length; i++)
            {
                //冲突的两种形式 1.同一时刻在同一个点 2.对方的下一位置为自己的当前位置
                //若对方的位置是自己的必经之路，然后相互等待，死锁？？
                //所以第2种冲突方式 机器人2重新规划路径如果不行 不能选择等待 得离开自己的位置 但是如果规划路径不行 代表的是
                if (route1[i].X == route2[i].X && route1[i].Y == route2[i].Y ||
                    i + 1 < length && route1[i + 1].X == route2[i].X && route1[i + 1].Y == route2[i].Y && route1[i].X == route2[i + 1].X && route1[i].Y == route2[i + 1].Y)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检测所有机器人的路线冲突
        /// </summary>
        /// <param name="forsee">向前看几步</param>
        public List<RouteConflictInfo> DetectConflict(int forsee = 5)
        {
            // 存储发生冲突的信息
            List<RouteConflictInfo> conflictInfos = new();
            // 有哪些机器人需要检测冲突
            List<Robot> robotsToDetect = new();
            // 存储每一个机器人的路线
            List<IList<Point>> routes = new();
            foreach (Robot r in Robots)
            {
                if (r.Route == null || r.Route.Count == 0)
                    continue;
                robotsToDetect.Add(r);
                routes.Add(r.Route);
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
                                conflictDict[routes[j][i]],//和第几个机器人碰撞
                                robotsToDetect[j],//第j个机器人的信息
                                routes[j][i],//碰撞的地方
                                i//未来i步碰撞
                            ));
                        }
                        else
                        {
                            conflictDict.Add(routes[j][i], robotsToDetect[j]);
                        }
                        // 处理两个机器人互相穿过对方的情况
                        if (routes[j].Count > i + 1)
                        {
                            for (int k = j + 1; k < routes.Count; ++k)//和后面的每个机器人比较
                            {
                                if (routes[k].Count > i + 1)//机器人的路径中要有第i+1步
                                {
                                    if (routes[k][i].X == routes[j][i + 1].X && routes[k][i].Y == routes[j][i + 1].Y
                                        && routes[k][i + 1].X == routes[j][i].X && routes[k][i + 1].Y == routes[j][i].Y)
                                    {
                                        conflictInfos.Add(new RouteConflictInfo(
                                            conflictDict[routes[k][i]],
                                            robotsToDetect[k],
                                            routes[j][i], // 碰撞的地方
                                            i + 1 // 未来i+1步碰撞
                                        ));
                                    }
                                }
                            }
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
        /*        public List<Point> GetCurrentCollision()
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
                }*/
        [Obsolete("仅用于控制台打印")]
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

            /*            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in RunningTasks)
                        {
                            Point pos = keyValuePair.Key.CurrentPosition;
                            Console.SetCursorPosition(pos.X * 2, pos.Y);
                            Console.Write("^ ");
                        }*/

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

            /*            foreach (Point position in GetCurrentCollision())
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(position.X * 2, position.Y);
                            Console.Write("X ");
                            Console.ResetColor();
                        }*/
        }
        /// <summary>
        /// 获取一个合适的机器人接受此任务
        /// </summary>
        /// <param name="point">任务的开始点</param>
        /// <returns></returns>
        private Robot FindSuitableRobot(Point point)
        {
            Robot temp = null;
            int minDistance = int.MaxValue;

            foreach (Robot robot in Robots)
            {
                if (robot.State == RobotState.Idle || robot.State == RobotState.Returning)
                {
                    // 对于空闲和返回状态，此机器人可以立即被指派新的任务
                    int distance = Math.Abs(robot.CurrentPosition.X - point.X) + Math.Abs(robot.CurrentPosition.Y - point.Y);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        temp = robot;
                    }
                }
                else if (robot.State == RobotState.Running)
                {
                    // 对于运行状态的机器人，需要等待其执行完当前任务再指派新任务
                    int distance = Math.Abs(robot.Route.Last().X - point.X) + Math.Abs(robot.Route.Last().Y - point.Y) + robot.Route.Count;
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        temp = robot;
                    }
                }
            }
            return temp;
        }


    }
}
