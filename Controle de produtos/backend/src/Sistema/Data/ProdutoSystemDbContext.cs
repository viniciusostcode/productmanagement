using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema.Data.Map;
using Sistema.Models; // Certifique-se de que ApplicationUser está corretamente importado aqui

namespace Sistema.Data
{
    public class ProdutoSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public ProdutoSystemDbContext(DbContextOptions<ProdutoSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProdutoModel> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProdutoMap());

            // Configuração do relacionamento entre ProdutoModel e ApplicationUser

            modelBuilder.Entity<ProdutoModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Preco);
                entity.Property(e => e.Situacao);
                entity.Property(e => e.Quantidade);
                entity.Property(e => e.Data);
                entity.Property(e => e.Produto);

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.IdUsuario);

            });
        }
    }
}