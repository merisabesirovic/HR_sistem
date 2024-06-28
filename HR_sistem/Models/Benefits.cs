using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_sistem.Models
{
    public class Benefits
    {
        public int ID { get; set; }
        public string BenefitName { get; set; }
        public DateTime? GrantedDate { get; set; }
        public DateTime? EndedDate { get; set; }


    }
}
