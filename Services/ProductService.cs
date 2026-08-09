using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class ProductService : IProductService
{
    public List<Product> GetAll(bool onlyAvailable)
    {
        IEnumerable<Product> products = FakeWarehouseStore.Products
            .Where(p => !p.IsArchived);

        if (onlyAvailable)
        {
            products = products.Where(p => p.QuantityInStock > 0);
        }

        return products
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public Product? GetById(Guid id)
    {
        return FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id && !p.IsArchived);
    }

    public List<Product> Search(string? name, string? supplier)
    {
        IEnumerable<Product> products = FakeWarehouseStore.Products
            .Where(p => !p.IsArchived);

        if (!string.IsNullOrWhiteSpace(name))
        {
            products = products.Where(p =>
                p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            products = products.Where(p =>
                p.SupplierName.Contains(
                    supplier,
                    StringComparison.OrdinalIgnoreCase));
        }

        return products
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public (bool Success, string? Error, Product? Product) Create(
        CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return (false, "Product name is required.", null);
        }

        if (string.IsNullOrWhiteSpace(request.SKU))
        {
            return (false, "SKU is required.", null);
        }

        if (request.Price <= 0)
        {
            return (false, "Price must be greater than 0.", null);
        }

        if (request.QuantityInStock < 0)
        {
            return (false, "Quantity cannot be negative.", null);
        }

        bool duplicateSku = FakeWarehouseStore.Products
            .Any(p => p.SKU.Equals(
                request.SKU,
                StringComparison.OrdinalIgnoreCase));

        if (duplicateSku)
        {
            return (false, "A product with this SKU already exists.", null);
        }

        DateTime now = DateTime.UtcNow;

        Product product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierName = request.SupplierName,
            ExpiryDate = request.ExpiryDate,
            IsArchived = false,
            CreatedAt = now,
            LastUpdatedAt = now
        };

        FakeWarehouseStore.Products.Add(product);

        return (true, null, product);
    }

    public (bool Success, string? Error, Product? Product) UpdateQuantity(
        Guid id,
        int quantity)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        if (quantity < 0)
        {
            return (false, "Quantity cannot be negative.", null);
        }

        product.QuantityInStock = quantity;
        product.LastUpdatedAt = DateTime.UtcNow;

        return (true, null, product);
    }

    public (bool Success, string? Error, Product? Product) UpdatePrice(
        Guid id,
        decimal price)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        if (price <= 0)
        {
            return (false, "Price must be greater than 0.", null);
        }

        decimal oldPrice = product.Price;

        product.Price = price;
        product.LastUpdatedAt = DateTime.UtcNow;

        Console.WriteLine(
            $"Product {product.Id} price changed from {oldPrice} to {price}");

        return (true, null, product);
    }

    public (bool Success, string? Error, Product? Product) Archive(Guid id)
    {
        Product? product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        if (product.IsArchived)
        {
            return (false, "Product is already archived.", null);
        }

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;

        return (true, null, product);
    }

    public (bool Success, string? Error, Product? Product) AssignSupplier(
        Guid productId,
        Guid supplierId)
    {
        Product? product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        if (product.IsArchived)
        {
            return (false, "Archived products cannot be assigned a supplier.", null);
        }

        Supplier? supplier = FakeWarehouseStore.Suppliers
            .FirstOrDefault(s => s.Id == supplierId);

        if (supplier == null)
        {
            return (false, "Supplier not found.", null);
        }

        if (!supplier.IsActive)
        {
            return (false, "Supplier is not active.", null);
        }

        product.SupplierId = supplier.Id;
        product.SupplierName = supplier.Name;
        product.LastUpdatedAt = DateTime.UtcNow;

        return (true, null, product);
    }
}