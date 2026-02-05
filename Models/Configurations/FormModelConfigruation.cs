using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FormModelConfiguration : IEntityTypeConfiguration<FormModel>
{
    public void Configure(EntityTypeBuilder<FormModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("Orders");
    }
}