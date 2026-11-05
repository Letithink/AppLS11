using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Roles.Queries;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;

internal sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRolesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return roles.Select(r => r.ToDto());
    }
}

