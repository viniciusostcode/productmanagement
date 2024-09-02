using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema.Data.Map;
using Sistema.Models;

namespace Sistema.Data
{
    public class ProductSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public ProductSystemDbContext(DbContextOptions<ProductSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductMap());


            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price);
                entity.Property(e => e.Situation);
                entity.Property(e => e.Quantity);
                entity.Property(e => e.Date);
                entity.Property(e => e.Product);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.IdUser);

            });
        }
    }
}