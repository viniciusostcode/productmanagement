using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Models;

namespace ProductManagement.Data.Map
{
    public class ProductMap : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price);
            builder.Property(x => x.Situation);
            builder.Property(x => x.Date);
            builder.Property(x => x.Product);

            builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.IdUser)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
