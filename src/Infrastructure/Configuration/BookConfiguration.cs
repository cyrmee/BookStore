using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Book)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();

        builder.HasMany(e => e.BookCategories)
            .WithOne(e => e.Book)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();
    }
}
