using Microsoft.EntityFrameworkCore;

namespace WarehouseManagement.Api.DatabaseFirst;

public partial class WarehouseDbFirstContext : DbContext
{
    public WarehouseDbFirstContext()
    {
    }

    public WarehouseDbFirstContext(
        DbContextOptions<WarehouseDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products", "public");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnName("name");

            entity.Property(e => e.SKU)
                .HasColumnName("sku");

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.Price)
                .HasColumnName("price");

            entity.Property(e => e.QuantityInStock)
                .HasColumnName("quantity");

            entity.Property(e => e.SupplierId)
                .HasColumnName("SupplierID");

            entity.Property(e => e.ExpiryDate)
                .HasColumnName("ExpiryDate");

            entity.Property(e => e.IsArchived)
                .HasColumnName("ISArchived");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdat");

            entity.Property(e => e.LastUpdatedAt)
                .HasColumnName("lastupdate");

            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(e => e.SupplierId)
                .HasPrincipalKey(s => s.Id);
        });


        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier", "public");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .HasColumnName("Name");

            entity.Property(e => e.Country)
                .HasColumnName("Country");

            entity.Property(e => e.ContactEmail)
                .HasColumnName("ContactEmail");

            entity.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber");

            entity.Property(e => e.IsActive)
                .HasColumnName("IsActive");
        });


        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImage", "public");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductID");

            entity.Property(e => e.FileName)
                .HasColumnName("FileName");

            entity.Property(e => e.FilePath)
                .HasColumnName("FilePath");

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(e => e.ProductId)
                .HasPrincipalKey(p => p.Id);
        });
    }

    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}