using AgiliFood.Application.Dtos;
using AgiliFood.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgiliFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _service;

    public ProductCategoryController(IProductCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();

        if (categories == null || !categories.Any())
        {
            return NotFound("No product categories found.");
        }

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound($"Product category with ID {id} not found.");
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            return BadRequest("Product category data is required.");
        }

        var createdCategory = await _service.CreateAsync(categoryDto);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto categoryDto)
    {
        if (categoryDto == null || categoryDto.Id != id)
        {
            return BadRequest("Product category data is invalid.");
        }
        var updatedCategory = await _service.UpdateAsync(categoryDto);
        if (updatedCategory == null)
        {
            return NotFound($"Product category with ID {id} not found.");
        }
        return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

}
