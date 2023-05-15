using Application.Intefaces;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Token;
using Domain.Models.UserCredentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] UserCredential credential)
    {
        string HashPassword = _userRepository.ComputeHash(credential.Password);
        User user = (await _userRepository.GetAsync(x => x.Username == credential.UserName &&
                                                         x.Password == HashPassword));


        if (user is null)
        {
            return NotFound("User not found!");
        }
        Token token = new()
        {
            AccessToken = await _tokenService.CreateAccessToken(user),
            RefreshToken = "1243h2k3jhf2j3h4k"
        };
        return Ok(token);
    }

    [HttpPost]
    [Route("Create")]
    //[Authorize(Roles = "CreateUser")]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        return await _userRepository.CreateAsync(user) ? Ok() : BadRequest();

    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var res = (await _userRepository.GetAllAsync()).Include(x => x.UserRoles).Select(x => new
        {
            x.UsersId,
            x.Username,
            Role = x.UserRoles.Select(t => new
            {
                t.Role.Id,
                t.Role.Name
            })
        });
        return Ok(res);

    }

}
