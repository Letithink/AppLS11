using AppLS10.application.DTOs;
using AppLS10.application.UseCases.Users.Commands;
using AppLS10.application.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppLS10.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _mediator.Send(new RegisterCommand(dto));
        if (user == null)
            return Conflict(new { message = "El usuario o el correo ya existen." });

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _mediator.Send(new LoginQuery(dto));
        if (result == null)
            return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

        return Ok(result);
    }
}
