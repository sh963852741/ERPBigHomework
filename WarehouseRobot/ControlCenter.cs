using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    public class ControlCenter
    {
        public List<Robot> robots;
        public Dictionary<Robot, TransportTask> runningTasks;
        /// <summary>
        /// 计算下一个时间点时机器人的信息
        /// </summary>
        public void NextTick()
        {
            foreach(Robot r in robots)
            {
                r.Move();
                if(r.State == Enum.RobotState.Finished)
                {

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
        public Stack<(uint,uint)> CalcPath(Robot robot, TransportTask task)
        {
            throw new NotImplementedException();
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
    }
}
