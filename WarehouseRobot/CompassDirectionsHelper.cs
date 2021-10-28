using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    public static class CompassDirectionsHelper
    {
        private static IList<CompassDirections> AllCompassDirections = new List<CompassDirections>();

        #region Static Ctor
        static CompassDirectionsHelper()
        {
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.East);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.West);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.South);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.North);

            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.SouthEast);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.SouthWest);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.NorthEast);
            CompassDirectionsHelper.AllCompassDirections.Add(CompassDirections.NorthWest);
        }
        #endregion

        #region GetAllCompassDirections
        public static IList<CompassDirections> GetAllCompassDirections()
        {
            return CompassDirectionsHelper.AllCompassDirections;
        }
        #endregion
    }
}
