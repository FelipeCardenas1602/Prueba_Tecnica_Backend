using Microsoft.AspNetCore.Mvc;
using InventoryService.Data;
using InventoryService.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("inventories")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ProductServiceClient _productClient;

    public InventoryController(AppDbContext context, ProductServiceClient productClient)
    {
        _context = context;
        _productClient = productClient;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetCantidad(int productId)
    {
        var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductoId == productId);
        if (inventory == null) return NotFound();

        return Ok(new
        {
            data = new
            {
                type = "inventory",
                id = inventory.Id,
                attributes = new
                {
                    producto_id = inventory.ProductoId,
                    cantidad = inventory.Cantidad
                }
            }
        });
    }

    [HttpPost("{productId}/update")]
    public async Task<IActionResult> UpdateCantidad(int productId, [FromQuery] int cantidad)
    {
        if (!await _productClient.ProductExistsAsync(productId))
            return NotFound(new { errors = new[] { new { detail = "Producto no encontrado" } } });

        var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductoId == productId);
        if (inventory == null)
        {
            inventory = new Inventory { ProductoId = productId, Cantidad = cantidad };
            _context.Inventories.Add(inventory);
        }
        else
        {
            inventory.Cantidad = cantidad;
        }

        await _context.SaveChangesAsync();
        Console.WriteLine($"[EVENTO] Cantidad actualizada para producto {productId}: {cantidad}");

        return NoContent();
    }
}

