using Application.Intefaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productService;

    public ProductController(IProductRepository productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Route("[action]")]
    [Authorize(Roles = "ProductGet")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {

            Product product = await _productService.GetAsync(x => x.ProductId == id);

            return Ok(new Response() { Result = product });
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response()
            {
                Message = e.Message,
                StatusCode = 500,
                IsSuccess = false,
            });
        }
    }
    [HttpGet]
    [Route("[action]")]
    [Authorize(Roles = "ProductGetAll")]
    public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 10)
    {
        try
        {
            IQueryable<Product> Products = await _productService.GetAllAsync();

            var products = Products.OrderBy(x => x.ProductId).Skip((page - 1) * pageSize).Take(pageSize);

            Response res = new()
            {
                Result = products,
                pageSize = pageSize,
                page = page
            };
            return Ok(res);
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response()
            {
                Message = e.Message,
                StatusCode = 500,
                IsSuccess = false,
            });
        }

    }

    [HttpPost]
    [Route("[action]")]
    [Authorize(Roles = "ProductCreate")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            bool IsSuccess = await _productService.CreateAsync(product);

            if (IsSuccess)
            {
                return Ok(product);
            }
        }
        return BadRequest();

    }

    [HttpPut]
    [Route("[action]")]
    [Authorize(Roles = "ProductUpdate")]
    public async Task<IActionResult> Update([FromBody] Product entity)
    {
        try
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = await _productService.UpdateAsync(entity);
                if (isSuccess)
                    return Ok(new Response()
                    {
                        Result = entity
                    });
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(new Response()
            {
                Message = ex.Message,
                IsSuccess = false,
                StatusCode = 400
            });
        }

    }

    [HttpDelete]
    [Route("[action]")]
    [Authorize(Roles = "ProductDelete")]
    public async Task<IActionResult> Delete([FromQuery] int Id)
    {
        try
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = await _productService.DeleteAsync(Id);
                if (isSuccess)
                    return Ok(new Response());
            }
            return BadRequest(new Response()
            {
                Message = "Not deleted",
                IsSuccess = false,
                StatusCode = 400
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new Response()
            {
                Message = ex.Message,
                IsSuccess = false,
                StatusCode = 400
            });
        }

    }
}
