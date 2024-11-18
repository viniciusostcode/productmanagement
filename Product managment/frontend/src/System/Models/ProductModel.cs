namespace frontend.Models
{
    public class ProductModel
    {
        public int? id { get; set; }
        public decimal price { get; set; }
        public string situation { get; set; }
        public int quantity { get; set; }
        public string currencyCode { get; set; }
        public DateTime? date { get; set; }
        public string? product { get; set; }
        public string? idUser { get; set; }
        public ApplicationUser? user { get; set; }
    }
}
