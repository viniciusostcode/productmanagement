using backend.Models.Enums;

namespace backend.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public decimal Price { get; set; }
        public string Situation { get; set; }
        public int Quantity { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? Date { get; set; }
        public string? Product { get; set; }
        public string? IdUser { get; set; }  
        public virtual ApplicationUser? User { get; set; } 
    }
}
