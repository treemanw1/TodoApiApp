using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.Entities;
using TodoApi.Repositories;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ITokenService tokenService,
    IEmailService emailService,
    IUserRepository userRepository,
    ITokenRepository tokenRepository
) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailService _emailService = emailService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;


    [HttpPost]
    public async Task<IActionResult> Authenticate(string email)
    {
        if (!_emailService.ValidEmail(email))
            return BadRequest("Invalid email.");
        var user = await _userRepository.GetUser(email);
        if (user is null)
        {
            user = new User { Email = email };
            await _userRepository.AddUser(user);
        }
        var tokenString = _tokenService.GenerateMagicToken(user.Id);
        await _emailService.SendMagicLinkEmail(email, tokenString);
        var token = new Token
        {
            User = user,
            Value = tokenString,
        };
        await _tokenRepository.AddToken(token);
        return Ok();
    }

    [HttpPost("Validate")]
    public async Task<IActionResult> Validate(string tokenString)
    {
        var principal = _tokenService.ValidateJwtToken(tokenString);
        if (principal is null)
            return BadRequest("Invalid token.");
        var userIdClaim = principal.FindFirst(claim => claim.Type == "userId");
        if (userIdClaim is null)
            return BadRequest("Invalid token: userId claim not found.");

        var userId = int.Parse(userIdClaim.Value);
        var user = await _userRepository.GetUser(userId);
        if (user is null)
            return BadRequest("Invalid token: user not found.");

        var token = await _tokenRepository.GetToken(tokenString); 
        if (token is null)
            return BadRequest("Invalid token: token not found.");
        if (token.Used)
            return BadRequest("Invalid token: token has already been used.");
        if (DateTime.UtcNow > token.Expiration)
            return BadRequest("Invalid token: token has expired.");

        token.Used = true;
        await _tokenRepository.UpdateToken(token);

        var accessToken = _tokenService.GenerateAccessToken(user.Id);
        return Ok(accessToken);
    } 
}
