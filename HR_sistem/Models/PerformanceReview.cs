using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_sistem.Models
{
    public class PerformanceReview
    {
        public int ID { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Score { get; set; }
        public string Comments { get; set; }
    }
}
