using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot.Enum
{
    /// <summary>
    /// 区块状态
    /// </summary>
    public enum ZoneState
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty,
        /// <summary>
        /// 有障碍物
        /// </summary>
        Blocked,
        /// <summary>
        /// 被机器人阻挡
        /// </summary>
        /// <remarks>此状态弃用</remarks>
        Occupied
    }
}
