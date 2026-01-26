using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryModelConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ProductCategoryModel>
{
    public void Configure(EntityTypeBuilder<ProductCategoryModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("Categories");
    }
}