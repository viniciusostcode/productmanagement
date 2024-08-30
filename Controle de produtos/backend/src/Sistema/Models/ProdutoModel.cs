using Sistema.Models.Enums;

namespace Sistema.Models
{
    public class ProdutoModel
    {
        public int? Id { get; set; }
        public string Preco { get; set; }
        public string Situacao { get; set; }
        public int Quantidade { get; set; }
        public DateTime? Data { get; set; }
        public string? Produto { get; set; }
        public string? IdUsuario { get; set; }  
        public virtual ApplicationUser? Usuario { get; set; } 

    }
}
