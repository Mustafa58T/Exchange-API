namespace Exchange_API.Dto
{
    public class ProductRequestDto
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int ProductStatus { get; set; }
        public string ProductFeatures { get; set; }
        public string ProductPrefence { get; set; }
        public string ProductImage { get; set; }
    }
}
