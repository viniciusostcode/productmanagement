using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Data.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "The price is required.")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "The situation is required.")]
        public string Situation { get; set; }
        [Required(ErrorMessage = "The quantity is required.")]
        public int? Quantity { get; set; }
        [Required(ErrorMessage = "The currency is required.")]
        public string CurrencyCode { get; set; }
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "The name is required.")]
        public string? Product { get; set; }
        public string? IdUser { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
