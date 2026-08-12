using System;
using System.Collections.Generic;

namespace WarehouseManagement.Api.DatabaseFirst;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public long QuantityInStock { get; set; }

    public int? SupplierId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<ProductImage> ProductImages { get; set; }
        = new List<ProductImage>();
}