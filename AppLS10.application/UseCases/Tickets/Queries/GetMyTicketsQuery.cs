using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Tickets.Queries;

public record GetMyTicketsQuery(Guid UserId) : IRequest<IEnumerable<TicketDto>>;

internal sealed class GetMyTicketsQueryHandler : IRequestHandler<GetMyTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMyTicketsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetMyTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetByUserIdAsync(request.UserId);
        return tickets.Select(t => t.ToDto());
    }
}

