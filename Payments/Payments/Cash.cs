using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments
{
    public class Cash
    {
        public int Number { get; set; }
        public DateTime TheDate { get; set; }
        public int Amount { get; set; }
        public int Remainder { get; set; }
    }
}
