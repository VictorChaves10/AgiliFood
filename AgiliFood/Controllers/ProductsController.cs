using AgiliFood.Application.Dtos;
using AgiliFood.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgiliFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{

    private readonly IProductService _service;
    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();
        if (products == null || !products.Any())
        {
            return NotFound("No products found.");
        }
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound($"Product with ID {id} not found.");
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Product data is required.");
        }
        var createdProduct = await _service.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
    {
        if (productDto == null || productDto.Id != id)
        {
            return BadRequest("Product data is invalid.");
        }

        var updatedProduct = await _service.UpdateAsync(productDto);

        if (updatedProduct == null)
        {
            return NotFound($"Product with ID {id} not found.");
        }
        
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound($"Product with ID {id} not found.");
        }
        
        return NoContent();
    }
}
