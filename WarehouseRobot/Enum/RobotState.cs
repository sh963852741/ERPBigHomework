using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot.Enum
{
    public enum RobotState
    {
        /// <summary>
        /// 在出生点，没事干
        /// </summary>
        Idle,
        /// <summary>
        /// 执行运输任务中
        /// </summary>
        Running,
        Broken,
        Shutdown,
        /// <summary>
        /// 装卸货中
        /// </summary>
        Oprating,
        /// <summary>
        /// 正在返回
        /// </summary>
        Returning,
        Unknown
    }
}
