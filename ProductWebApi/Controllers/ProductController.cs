using Application.Intefaces;
using Application.Models;
using Domain.Models;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductWebApi.Filters;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productService;
    private readonly IMemoryCache _memoryCache;
    private readonly IAppCache _LazyCache;
    private const string My_Key = "My_Key";


    public ProductController(IProductRepository productService, IMemoryCache memoryCache, IAppCache lazyCache)
    {
        _productService = productService;
        _memoryCache = memoryCache;
        _LazyCache = lazyCache;
    }

    [HttpGet]
    [Route("[action]")]
    [Authorize(Roles = "ProductGet")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {

            Product product = await _productService.GetAsync(x => x.ProductId == id);

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    [HttpGet]
    [Route("[action]")]
    [LazyCachePro(5, 10)]
    public async Task<ActionResult<Response<PaginatedList<Product>>>> GetAllProducts(int page = 1, int pageSize = 10)
    {

        List<string> strings = new() { "Salom", "Hi", "Anahasiyo" };
        List<string>? isFound = _memoryCache.GetOrCreate(My_Key, c =>
        {
            c.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
            return strings;
        });


        Ok(isFound);

        var isFound2= _LazyCache.GetOrAdd(My_Key, c =>
        {
            c.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
            return strings;
        });
        

        //if (!isFound)
        //{

        //    var cachetTime = new MemoryCacheEntryOptions()
        //        .SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

        //    _memoryCache.Set(My_Key, strings, cachetTime);
        //    return Ok(strings); 
        //}
        //else
        //{
        //    return Ok(result);
        //}
        IQueryable<Product> Products = await _productService.GetAllAsync();

        PaginatedList<Product> products = await PaginatedList<Product>.CreateAsync(Products, page, pageSize);

        Response<PaginatedList<Product>> res = new()
        {
            Result = products
        };
        return Ok(res);


    }


    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<Response<PaginatedList<Product>>>> Search(string text, int page = 1, int pageSize = 10)
    {
        var Products = await _productService.GetAllAsync(x => x.Price.ToString().Contains(text)
                                                   || x.Description.Contains(text)
                                                   || x.Name.Contains(text));

        PaginatedList<Product> products = await PaginatedList<Product>.CreateAsync(Products, page, pageSize);

        Response<PaginatedList<Product>> res = new()
        {
            Result = products
        };
        return Ok(res);


    }
    [HttpPost]
    [Route("[action]")]
    //[Authorize(Roles = "ProductCreate")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            bool IsSuccess = await _productService.CreateAsync(product);
            _memoryCache.Remove(My_Key);
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
                    return Ok();
            }
            return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }

    }

    [HttpDelete]
    [Route("[action]")]
    [Authorize(Roles = "ProductDelete")]
    public async Task<IActionResult> Delete([FromQuery] int Id)
    {
        if (ModelState.IsValid)
        {
            bool isSuccess = await _productService.DeleteAsync(Id);
            if (isSuccess)
                return Ok();
        }
        return BadRequest("Not deleted");


    }
}
