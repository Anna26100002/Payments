using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments
{
    public class Payment
    {
        public int Id { get; set; }
        public int Cash { get; set; }
        public int OrderNumber { get; set; }
        public int Amount { get; set; }
    }
}
