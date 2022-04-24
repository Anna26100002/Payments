using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments
{
    public class Orders
    {
        public int OrderNumber { get; set; }
        public DateTime TheDate { get; set; }
        public int Amount { get; set; }
        public int PaymentAmount { get; set; }
    }
}
