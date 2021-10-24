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
            for(int i=1;i<=30;++i)
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
                r.Move();
                if (r.State == Enum.RobotState.Finished)
                {
                    runningTasks.Remove(r);
                }
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
                if(from.Item1< to.Item1)
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
        public void DetectConflict()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 解决路线冲突的问题
        /// </summary>
        public void ResolveConflict()
        {
            throw new NotImplementedException();
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
        }
    }
}
