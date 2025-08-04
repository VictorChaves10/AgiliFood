namespace AgiliFood.Business.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string BarCode { get; set; }
        
        public string Image { get; set; }

        public long ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

    }
}
