using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot.Utility
{
    public static class CompassDirectionsHelper
    {
        private static IList<CompassDirections> allCompassDirections = new List<CompassDirections>();


        static CompassDirectionsHelper()
        {
            allCompassDirections.Add(CompassDirections.East);
            allCompassDirections.Add(CompassDirections.West);
            allCompassDirections.Add(CompassDirections.South);
            allCompassDirections.Add(CompassDirections.North);

            allCompassDirections.Add(CompassDirections.SouthEast);
            allCompassDirections.Add(CompassDirections.SouthWest);
            allCompassDirections.Add(CompassDirections.NorthEast);
            allCompassDirections.Add(CompassDirections.NorthWest);
        }

        /// <summary>
        /// 获取所有合理的方向，即除了<see cref="CompassDirections.NotSet"/>的所有方向
        /// </summary>
        /// <returns></returns>
        public static IList<CompassDirections> GetReasonableCompassDirections()
        {
            return allCompassDirections;
        }
    }
}
