namespace Exchange_API.Model
{
    public class Product
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int ProductStatus { get; set; }
        public string ProductFeatures { get; set; }
        public string ProductPrefence { get; set; }
        public string ProductImage { get; set; }

    }
}
