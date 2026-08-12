namespace WarehouseManagement.Api.Models;

public class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<Product> Products { get; set; }
        = new List<Product>();
}