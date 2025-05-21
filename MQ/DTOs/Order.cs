using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.DTOs
{

    public class Order
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
