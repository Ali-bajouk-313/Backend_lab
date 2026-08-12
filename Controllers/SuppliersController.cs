using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(
        ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers =
            await _supplierService.GetAllAsync();

        return Ok(suppliers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier =
            await _supplierService.GetByIdAsync(id);

        if (supplier == null)
        {
            return NotFound(new
            {
                message = "Supplier not found."
            });
        }

        return Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSupplierRequest request)
    {
        try
        {
            var supplier =
                await _supplierService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = supplier.SupplierId },
                supplier);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result =
            await _supplierService.DeactivateAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Supplier not found."
            });
        }

        return NoContent();
    }
}