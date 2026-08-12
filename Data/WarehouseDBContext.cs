using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Data;

public class WarehouseDBContext : DbContext
{
    public WarehouseDBContext(
        DbContextOptions<WarehouseDBContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(p => p.SKU)
                .IsUnique();

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.SupplierId);

            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.ContactEmail)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(i => i.ProductImageId);

            entity.Property(i => i.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(i => i.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}