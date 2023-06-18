using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Configuration;

namespace Infrastructure;

public class BookStoreDbContext : IdentityDbContext<User>
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        => _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ModelBuilderConfiguration.Apply(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<BookCategory> BookCategories { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrdersDetails { get; set; } = default!;
    public DbSet<JwtTokens> JwtTokens{ get; set; } = default!;
}