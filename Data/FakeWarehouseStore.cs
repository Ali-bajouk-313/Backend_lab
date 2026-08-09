using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Data;

public static class FakeWarehouseStore
{
    public static List<Product> Products { get; } = new()
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Dell Laptop",
            SKU = "LAP-001",
            Description = "Business laptop",
            Price = 899.99m,
            QuantityInStock = 15,
            SupplierName = "Dell Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            LastUpdatedAt = DateTime.UtcNow.AddDays(-2)
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Wireless Mouse",
            SKU = "MOU-001",
            Description = "Wireless optical mouse",
            Price = 29.99m,
            QuantityInStock = 50,
            SupplierName = "Tech Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-9),
            LastUpdatedAt = DateTime.UtcNow.AddDays(-1)
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mechanical Keyboard",
            SKU = "KEY-001",
            Description = "RGB mechanical keyboard",
            Price = 79.99m,
            QuantityInStock = 30,
            SupplierName = "Tech Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-8),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Barcode Scanner",
            SKU = "SCN-001",
            Description = "Warehouse barcode scanner",
            Price = 149.99m,
            QuantityInStock = 12,
            SupplierName = "Scanner Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Laser Printer",
            SKU = "PRN-001",
            Description = "Office laser printer",
            Price = 299.99m,
            QuantityInStock = 8,
            SupplierName = "Printer Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-6),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Dell Monitor",
            SKU = "MON-001",
            Description = "24 inch Full HD monitor",
            Price = 199.99m,
            QuantityInStock = 20,
            SupplierName = "Dell Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "USB-C Hub",
            SKU = "HUB-001",
            Description = "Multi-port USB-C hub",
            Price = 49.99m,
            QuantityInStock = 0,
            SupplierName = "Tech Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Network Switch",
            SKU = "NET-001",
            Description = "24 port network switch",
            Price = 249.99m,
            QuantityInStock = 10,
            SupplierName = "Network Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-3),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Webcam",
            SKU = "CAM-001",
            Description = "Full HD webcam",
            Price = 69.99m,
            QuantityInStock = 25,
            SupplierName = "Camera Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            LastUpdatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "External SSD",
            SKU = "SSD-001",
            Description = "1TB external SSD",
            Price = 119.99m,
            QuantityInStock = 18,
            SupplierName = "Storage Supplier",
            IsArchived = false,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            LastUpdatedAt = DateTime.UtcNow
        }
    };

    public static List<Supplier> Suppliers { get; } = new()
    {
        new Supplier
        {
            Id = Guid.NewGuid(),
            Name = "Dell Supplier",
            Country = "USA",
            ContactEmail = "contact@dellsupplier.com",
            PhoneNumber = "+1 555 100 1000",
            IsActive = true
        },

        new Supplier
        {
            Id = Guid.NewGuid(),
            Name = "Tech Supplier",
            Country = "China",
            ContactEmail = "contact@techsupplier.com",
            PhoneNumber = "+86 555 200 2000",
            IsActive = true
        },

        new Supplier
        {
            Id = Guid.NewGuid(),
            Name = "Network Supplier",
            Country = "Germany",
            ContactEmail = "contact@networksupplier.com",
            PhoneNumber = "+49 555 300 3000",
            IsActive = true
        }
    };
}