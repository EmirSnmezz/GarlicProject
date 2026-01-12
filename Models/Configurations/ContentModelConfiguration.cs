using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ContentModelConfiguration : IEntityTypeConfiguration<ContentModel>
{
    public void Configure(EntityTypeBuilder<ContentModel> builder)
    {
        builder.HasKey(contentModel => contentModel.Id);
        builder.ToTable("Contents");
    }
}