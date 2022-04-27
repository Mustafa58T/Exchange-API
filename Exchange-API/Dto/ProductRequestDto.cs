namespace Exchange_API.Dto
{
    public class ProductRequestDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int ProductStatus { get; set; }
        public string ProductFeatures { get; set; }
        public string ProductPrefence { get; set; }
        public string ProductTitle { get; set; }
        public string ProductLocation { get; set; }
    }
}
