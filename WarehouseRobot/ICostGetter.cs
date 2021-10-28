using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    /// <summary>
        /// ICostGetter 获取从当前节点向某个方向移动时的代价。
        /// </summary>
    public interface ICostGetter
    {
        int GetCost(Point currentNodeLoaction, CompassDirections moveDirection);
    }
}
