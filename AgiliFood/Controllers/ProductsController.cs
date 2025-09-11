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
            return NotFound("Não foi localizado os produtos.");

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound("Produto não localizado");

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        if (productDto == null)
            return BadRequest("O produto é obrigatório.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdProduct = await _service.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
    {
        if (productDto == null || productDto.Id != id)
            return BadRequest("Produto com id inválido");

        var updatedProduct = await _service.UpdateAsync(productDto);

        if (updatedProduct == null)
            return NotFound("Produto não localizado");


        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Produto não localizado");


        return NoContent();
    }
}
