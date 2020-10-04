using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariusSimke_TGWassignment.DataModels.Entities
{
   public class ConfigEntry
    {
        public int? NumberOfSystems { get; set; }
        public int? OrdersPerHour { get; set; }
        public int? OrderLinesPerOrder { get; set; }
        public TimeSpan? ResultStartTime { get; set; }
    }
}
