using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    public class TransportTask
    {
        public Guid id;
        public (uint, uint) from;
        public (uint, uint) to;
    }
}
