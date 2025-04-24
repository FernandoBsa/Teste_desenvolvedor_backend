using InsideStore.Domain.Enum;

namespace InsideStore.Domain.Entities
{
    public class Order : EntityBase
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Open;
        public decimal Total { get; set; }
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        private Order()
        {
            
        }
    }
}
