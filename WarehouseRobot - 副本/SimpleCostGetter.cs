using System.Drawing;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    /// <summary>
    /// 查找移动的代价。直线代价为10，斜线为14。
    /// </summary>
    public class SimpleCostGetter : ICostGetter
    {
        public int GetCost(Point currentNodeLoaction, CompassDirections moveDirection)
        {
            if (moveDirection == CompassDirections.NotSet)
            {
                return 0;
            }
            else if (moveDirection == CompassDirections.East || moveDirection == CompassDirections.West || moveDirection == CompassDirections.South || moveDirection == CompassDirections.North)
            {
                return 10;
            }
            else
            {
                return 14;
            }
        }
    }
}
