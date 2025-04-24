using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideStore.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal => UnitPrice * Quantity;
        public Order Order { get; set; }
        public Product Product { get; set; }

        private OrderItem()
        {
            
        }
    }
}
