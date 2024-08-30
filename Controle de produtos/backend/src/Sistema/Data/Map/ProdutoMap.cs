using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sistema.Models.Enums;
using System.Reflection.Emit;

namespace Sistema.Data.Map
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Preco);
            builder.Property(x => x.Situacao);
            builder.Property(x => x.Data);
            builder.Property(x => x.Produto);
        }
    }
}
