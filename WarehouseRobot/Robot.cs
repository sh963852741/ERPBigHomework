using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    /// <summary>
    /// 机器人
    /// </summary>
    public class Robot
    {
        /// <summary>
        /// 机器人ID，唯一标识
        /// </summary>
        public Guid Id
        {
            get; set;
        } = Guid.NewGuid();
        /// <summary>
        /// 记录机器人行动的路径
        /// </summary>
        public IList<Point> Route
        {
            get; set;
        }
        /// <summary>
        /// 机器人的历史路径
        /// </summary>
        public List<Point> History
        {
            get; set;
        } = new();
        /// <summary>
        /// 机器人状态
        /// </summary>
        public RobotState State
        {
            get; set;
        }
        /// <summary>
        /// 机器人当前位置
        /// </summary>
        public Point CurrentPosition
        {
            get; set;
        }

        /// <summary>
        /// 移动到下一格
        /// </summary>
        /// <returns>是否移动成功</returns>
        public bool Move()
        {
            if (State == RobotState.Running)
            {
                CurrentPosition = Route[0];
                Route.RemoveAt(0);
                History.Add(CurrentPosition);
                Console.WriteLine(Route.Count);
                if (Route.Count == 0)
                {
                    State = RobotState.Finished;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 为机器人设置一个运输任务并将状态设置为运行
        /// </summary>
        /// <param name="route">机器人的运输路线，含起点和终点</param>
        public void SetTask(IList<Point> route)
        {
            if (route == null)
                State = RobotState.Finished;
            else
            {
                Route = route;
                State = RobotState.Running;
            }
        }
        public void Reset()
        {
            State = RobotState.Idle;
            History.Clear();
            if (Route == null)
                return;
            Route.Clear();
        }
    }
}
