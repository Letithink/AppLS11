using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Tickets.Queries;

public record GetTicketsQuery : IRequest<IEnumerable<TicketDto>>;

internal sealed class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetAllAsync();
        return tickets.Select(t => t.ToDto());
    }
}

