using Application.Intefaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Route("[action]{id}")]
    public async Task<IActionResult> Get(int id)
    {
        await _productService.GetByIdAsync(id);

        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            bool IsSuccess = await _productService.CreateAsync(product);

            if (IsSuccess)
            {
                return Ok(JsonConvert.SerializeObject(product));

            }
        }
            return BadRequest();

    }
}
