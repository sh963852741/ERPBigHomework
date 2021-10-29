using System.Drawing;
using WarehouseRobot.Enum;

/*
 *  ---------→ X轴正方向
 *  |
 *  |
 *  |
 *  ↓
 * Y轴
 * 正方向
 */

namespace WarehouseRobot.Utility
{
    public static class GeometryHelper
    {
        /// <summary>
        /// GetAdjacentPoint 获取某个方向上的相邻点
        /// </summary>       
        public static Point GetAdjacentPoint(Point current, CompassDirections direction)
        {
            switch (direction)
            {
                case CompassDirections.North:
                    {
                        return new Point(current.X, current.Y - 1);
                    }
                case CompassDirections.South:
                    {
                        return new Point(current.X, current.Y + 1);
                    }
                case CompassDirections.East:
                    {
                        return new Point(current.X + 1, current.Y);
                    }
                case CompassDirections.West:
                    {
                        return new Point(current.X - 1, current.Y);
                    }
                case CompassDirections.NorthEast:
                    {
                        return new Point(current.X + 1, current.Y - 1);
                    }
                case CompassDirections.NorthWest:
                    {
                        return new Point(current.X - 1, current.Y - 1);
                    }
                case CompassDirections.SouthEast:
                    {
                        return new Point(current.X + 1, current.Y + 1);
                    }
                case CompassDirections.SouthWest:
                    {
                        return new Point(current.X - 1, current.Y + 1);
                    }
                default:
                    {
                        return current;
                    }
            }
        }
    }
}
