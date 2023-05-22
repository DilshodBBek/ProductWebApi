using Application.Extensions;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Token;
using Domain.Models.UserCredentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace ProductWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    readonly IDiagnosticContext _diagnosticContext;
    public UserController(IUserRepository userRepository, ITokenService tokenService, IDiagnosticContext diagnosticContext)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        Log.Verbose("UserController  is started.");
        _diagnosticContext = diagnosticContext;
    }



    [HttpPost]
    [AllowAnonymous]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] UserCredential credential)
    {
        string HashPassword = credential.Password.ComputeHash();
        User user = (await _userRepository.GetAsync(x => x.Username == credential.UserName &&
                                                         x.Password == HashPassword));

        Log.Warning("This is Warning");

        if (user is null)
        {
            return NotFound("Not found Objects");
        }
        Tokens token = await _tokenService.CreateTokensAsync(user);

        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Refresh")]
    public async Task<IActionResult> Refresh([FromBody] Tokens tokens)
    {
        var principal = _tokenService.GetClaimsFromExpiredToken(tokens.AccessToken);
        string? username = principal.Identity?.Name;
        if (username == null)
        {
            return NotFound("Refresh token not found!");
        }
        RefreshToken? savedRefreshToken = _tokenService.Get(x => x.Username == username &&
                                                  x.RefreshTokenValue == tokens.RefreshToken)
                                                 .FirstOrDefault();

        if (savedRefreshToken == null)
        {
            return BadRequest("Refresh token or Access token invalid!");
        }
        if (savedRefreshToken.ExpiredDate < DateTime.UtcNow)
        {
            _tokenService.Delete(savedRefreshToken);
            return StatusCode(405, "Refresh token already expired");
        }
        Tokens newTokens = await _tokenService.CreateTokensFromRefresh(principal, savedRefreshToken);

        return Ok(newTokens);

    }
    [HttpPost]
    [Route("Create")]
    // [Authorize(Roles = "CreateUser")]
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
    //[Authorize(Roles = "GetAllUsers")]
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

    [HttpPost]
    [AllowAnonymous]
    [Route("Logger")]
    [ValidateModel]
    public async Task<IActionResult> Logger([FromBody] UserCredential credential)
    {
        _diagnosticContext.Set("CatalogLoadTime", 1423);
        Log.Debug("this is LogDebug");
       await _userRepository.GetAllAsync();
        return Ok();
    }

}
