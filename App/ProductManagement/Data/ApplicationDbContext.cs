using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Map;
using ProductManagement.Data.Models;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

                entity.Property(e => e.Price)
                .IsRequired()
                .HasPrecision(18, 2);

                entity.Property(e => e.Situation)
                .IsRequired();

                entity.Property(e => e.Quantity)
                .IsRequired();

                entity.Property(e => e.CurrencyCode)
                .IsRequired();

                entity.Property(e => e.Date);
                entity.Property(e => e.Product)
                .IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.IdUser);

            });
        }
    }
}
