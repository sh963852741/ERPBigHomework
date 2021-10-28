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
        /// SimpleCostGetter ICostGetter接口的简化实现。直线代价为10， 斜线为14。
        /// </summary>
    public class SimpleCostGetter : ICostGetter
    {
        #region ICostGetter 成员

        public int GetCost(Point currentNodeLoaction, CompassDirections moveDirection)
        {
            if (moveDirection == CompassDirections.NotSet)
            {
                return 0;
            }

            if (moveDirection == CompassDirections.East || moveDirection == CompassDirections.West || moveDirection == CompassDirections.South || moveDirection == CompassDirections.North)
            {
                return 10;
            }

            return 14;
        }

        #endregion
    }
}
