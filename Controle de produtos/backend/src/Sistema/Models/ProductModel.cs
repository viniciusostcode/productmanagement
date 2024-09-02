using Sistema.Models.Enums;

namespace Sistema.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public string Price { get; set; }
        public string Situation { get; set; }
        public int Quantity { get; set; }
        public DateTime? Date { get; set; }
        public string? Product { get; set; }
        public string? IdUser { get; set; }  
        public virtual ApplicationUser? User { get; set; } 

    }
}
