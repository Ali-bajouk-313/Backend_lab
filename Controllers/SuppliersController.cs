using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // GET /api/suppliers
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Supplier> suppliers =
            _supplierService.GetAll();

        return Ok(suppliers);
    }

    // GET /api/suppliers/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id)
    {
        Supplier? supplier =
            _supplierService.GetById(id);

        if (supplier == null)
        {
            return NotFound(new
            {
                message = "Supplier not found."
            });
        }

        return Ok(supplier);
    }

    // POST /api/suppliers
    [HttpPost]
    public IActionResult Create(
        [FromBody] CreateSupplierRequest request)
    {
        var result =
            _supplierService.Create(request);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Supplier!.Id },
            result.Supplier);
    }

    // DELETE /api/suppliers/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult Deactivate(
        [FromRoute] Guid id)
    {
        var result =
            _supplierService.Deactivate(id);

        if (!result.Success)
        {
            return NotFound(new
            {
                message = result.Error
            });
        }

        return Ok(new
        {
            message = "Supplier deactivated successfully.",
            supplier = result.Supplier
        });
    }
}