using Application.Abstraction;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionController : ControllerBase
{
    private readonly IApplicationDbContext _permissionService;

    public PermissionController(IApplicationDbContext permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    [Route("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] string[] permissions)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var _permissions = new List<Permission>();
        foreach (string item in permissions)
        {
            _permissions.Add(new()
            {
                PermissionName = item
            });
        }
        bool result = _permissionService.Permissions.AddRangeAsync(_permissions).IsCompletedSuccessfully;
        if (result)
        {
            await _permissionService.SaveChangesAsync();
            return Ok();
        }
        return BadRequest();


    }
}
