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
        public Stack<(uint,uint)> Path
        {
            get; set;
        }
        public Stack<(uint, uint)> History
        {
            get; set;
        }
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
                CurrentPosition = Path.Pop();
                History.Push(CurrentPosition);

                if(Path.Count == 0)
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

        public void SetTask(Stack<(uint, uint)> path)
        {
            Path = path;
            State = RobotState.Running;
        }
    }
}
