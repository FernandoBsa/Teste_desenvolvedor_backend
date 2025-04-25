namespace InsideStore.Domain.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        private Product()
        {
           
        }
        
        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }
        
        public void DecreaseStock(int quantity)
        {
            Stock -= quantity;
        }
    }
}
