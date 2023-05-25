﻿using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Interceptors;

namespace Infrastructure;

public class BookStoreDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        => _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasMany(e => e.OrderDetails)
            .WithOne(e => e.Book)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();

            entity.HasMany(e => e.BookCategories)
            .WithOne(e => e.Book)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasMany(e => e.BookCategories)
            .WithOne(e => e.Category)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.BookId)
            .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(e => e.Orders)
            .WithOne(e => e.User)
            .HasPrincipalKey(e => e.UserName)
            .HasForeignKey(e => e.UserName)
            .IsRequired();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public virtual DbSet<Book> Books { get; set; } = default!;
    public virtual DbSet<BookCategory> BookCategories { get; set; } = default!;
    public virtual DbSet<Category> Categories { get; set; } = default!;
    public virtual DbSet<Order> Orders { get; set; } = default!;
    public virtual DbSet<OrderDetail> OrdersDetails { get; set; } = default!;
}