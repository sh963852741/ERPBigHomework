using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    public class RouteConflictInfo
    {
        public Robot Robot1
        {
            get;
        }
        public Robot Robot2
        {
            get;
        }
        public RHPoint Where
        {
            get;
        }
        public int Step
        {
            get;
        }

        public RouteConflictInfo(Robot r1,Robot r2, RHPoint where, int step)
        {
            Robot1 = r1;
            Robot2 = r2;
            Where = where;
            Step = step;
        }
    }
}
