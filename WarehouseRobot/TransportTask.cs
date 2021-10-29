using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    /// <summary>
    /// 表示一个需要被执行的运输任务
    /// </summary>
    public class TransportTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid id;
        /// <summary>
        /// 起点
        /// </summary>
        public Point from;
        /// <summary>
        /// 终点
        /// </summary>
        public Point to;
    }
}
