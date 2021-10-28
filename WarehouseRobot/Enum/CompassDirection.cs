using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot.Enum
{
    public enum CompassDirections
    {
        NotSet = 0,
        North = 1, //UP
        NorthEast = 2, //UP Right
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7,
        NorthWest = 8
    }
}
