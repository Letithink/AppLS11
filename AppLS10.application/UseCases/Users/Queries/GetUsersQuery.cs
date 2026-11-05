using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Users.Queries;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllWithRolesAsync();
        return users.Select(u => u.ToDto());
    }
}

