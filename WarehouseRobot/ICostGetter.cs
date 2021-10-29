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
        /// <summary>
        /// 获取从一个点移动到另一个点的花费
        /// </summary>
        /// <param name="currentNodeLocation">当前点的位置</param>
        /// <param name="moveDirection">移动方向</param>
        /// <returns>花费</returns>
        int GetCost(Point currentNodeLocation, CompassDirections moveDirection);
    }
}
