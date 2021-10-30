using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot.Utility
{
    public static class GridGenerator
    {
        private readonly static Random r = new();

        public static ZoneState[,] GetGrid(out IList<Point> obstaclePoints, int rowCount, int colCount)
        {
            obstaclePoints = new List<Point>();
            var grid = new ZoneState[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (r.Next() % 10 == 0)
                    {
                        grid[i, j] = ZoneState.Blocked;
                        obstaclePoints.Add(new Point(j, i));
                    }
                    else
                        grid[i, j] = ZoneState.Empty;
                }
            }
            return grid;
        }

        /// <summary>
        /// 指定障碍物位置，进行初始化。
        /// </summary>
        /// <param name="obstaclePoints">指定障碍物的位置</param>
        public static ZoneState[,] GetGrid(IList<Point> obstaclePoints, int rowCount, int colCount)
        {
            var grid = new ZoneState[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    grid[i, j] = ZoneState.Empty;
                }
            }
            foreach (Point pt in obstaclePoints)
            {
                grid[pt.Y, pt.X] = ZoneState.Blocked;
            }

            return grid;
        }
    }
}
