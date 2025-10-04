using AgiliFood.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgiliFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockItemController : ControllerBase
{
    private readonly IStockItemService _service;

    public StockItemController(IStockItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        return Ok("StockItemController - GetAll");
    }
}
