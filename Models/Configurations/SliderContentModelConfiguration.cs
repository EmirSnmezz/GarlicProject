using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class SliderContentModelConfiguration : IEntityTypeConfiguration<SliderModel>
{
    public void Configure(EntityTypeBuilder<SliderModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("Sliders");
    }
}