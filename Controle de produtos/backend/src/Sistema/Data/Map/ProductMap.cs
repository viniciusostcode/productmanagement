using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sistema.Models.Enums;
using System.Reflection.Emit;

namespace Sistema.Data.Map
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
        }
    }
}
