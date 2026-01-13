using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class SliderContentModelConfiguration : IEntityTypeConfiguration<SliderContentModel>
{
    public void Configure(EntityTypeBuilder<SliderContentModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("Sliders");
    }
}