using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasMany(e => e.BookCategories)
            .WithOne(e => e.Category)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();
    }
}
