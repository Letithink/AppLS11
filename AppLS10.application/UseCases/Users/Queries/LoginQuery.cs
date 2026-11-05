using AppLS10.application.DTOs;
using AppLS10.application.Services;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Users.Queries;

public record LoginQuery(LoginDto Dto) : IRequest<AuthResponseDto?>;

internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Dto.Username);
        if (user == null || user.PasswordHash != request.Dto.Password)
            return null;

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var token = _tokenService.GenerateToken(user.UserId, user.Username, roles);

        return new AuthResponseDto { Token = token, Username = user.Username, Roles = roles };
    }
}

