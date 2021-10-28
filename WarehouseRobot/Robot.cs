using System;
using System.Collections.Generic;
using System.Drawing;
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
        public IList<Point> Route
        {
            get; set;
        }
/*        public List<Point> History
        {
            get; set;
        } = new();*/
        public RobotState State
        {
            get; set;
        }
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
                //History.Add(CurrentPosition);
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
                //Console.WriteLine(route);
                State = RobotState.Running;
            }
        }
        public void Reset()
        {
            State = RobotState.Idle;
            //History.Clear();
            if (Route == null)
                return;
            Route.Clear();
            
        }
    }
}
