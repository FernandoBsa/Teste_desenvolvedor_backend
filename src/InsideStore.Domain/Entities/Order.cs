using InsideStore.Domain.Enum;

namespace InsideStore.Domain.Entities
{
    public class Order : EntityBase
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Open;
        public decimal Total { get; private set; }
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public Order()
        {
            
        }
        
        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                Items.Add(new OrderItem(product, quantity));
            }

            UpdateTotal();
        }

        public void RemoveItem(Guid productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            if (item.Quantity <= quantity)
                Items.Remove(item);
            else
                item.UpdateQuantity(item.Quantity - quantity);

            UpdateTotal();
        }

        public void Close()
        {
            foreach (var item in Items)
            {
                item.Product.DecreaseStock(item.Quantity);
            }

            Status = OrderStatus.Closed;
            ClosedAt = DateTime.UtcNow;
        }

        private void UpdateTotal() => Total = Items.Sum(i => i.SubTotal);
    }
}
