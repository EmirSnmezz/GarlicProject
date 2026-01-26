using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImageModelConfiguration : IEntityTypeConfiguration<ImageModel>
{
    public void Configure(EntityTypeBuilder<ImageModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("SliderImages");
    }
}