using System;
using System.Collections.Generic;
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
        public (uint, uint) Where
        {
            get;
        }
        public int Step
        {
            get;
        }

        public RouteConflictInfo(Robot r1,Robot r2,(uint,uint) where, int step)
        {
            Robot1 = r1;
            Robot2 = r2;
            Where = where;
            Step = step;
        }
    }
}
