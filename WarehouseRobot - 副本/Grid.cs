using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    [Obsolete]
    public class Grid
    {
        private static readonly Random r = new Random();
        public (uint, uint) Size
        {
            get
            {
                return ((uint)zones.GetLength(0), (uint)zones.GetLength(0));
            }
        }
        public ZoneState GetPositionState(uint row, uint col)
        {
            return zones[row, col];
        }

        public ZoneState[,] zones = new ZoneState[20, 20];
        public Grid(ZoneState[,] _grid)
        {
            zones = _grid;
        }
        public Grid()
        {
            uint maxRow = Size.Item1;
            uint maxCol = Size.Item2;
            for (int i = 0; i < maxRow; ++i)
            {
                for (int j = 0; j < maxCol; ++j)
                {
                    zones[i, j] = RandomZoneState();
                }
            }
        }

        private static ZoneState RandomZoneState()
        {
            if (r.Next() % 10 == 0)
                return ZoneState.Blocked;
            else
                return ZoneState.Empty;
        }
    }
}
