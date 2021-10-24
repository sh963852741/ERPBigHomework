using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    public class Robot
    {
        public Guid Id
        {
            get;set;
        }
        public Stack<(uint, uint)> Route
        {
            get; set;
        } = new();
        public Stack<(uint, uint)> History
        {
            get; set;
        } = new();
        public RobotState State
        {
            get; set;
        }
        public (uint, uint) CurrentPosition
        {
            get; set;
        }

        /// <summary>
        /// 移动到下一格
        /// </summary>
        /// <returns>是否移动成功</returns>
        public bool Move()
        {
            if(State == RobotState.Running)
            {
                CurrentPosition = Route.Pop();
                History.Push(CurrentPosition);

                if(Route.Count == 0)
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
        public void SetTask(Stack<(uint, uint)> route)
        {
            Route = route;
            State = RobotState.Running;
        }
        public void Reset()
        {
            History.Clear();
            Route.Clear();
            State = RobotState.Idle;
        }
    }
}
