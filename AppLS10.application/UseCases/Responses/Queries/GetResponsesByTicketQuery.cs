using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Responses.Queries;

public record GetResponsesByTicketQuery(Guid TicketId) : IRequest<IEnumerable<ResponseDto>?>;

internal sealed class GetResponsesByTicketQueryHandler : IRequestHandler<GetResponsesByTicketQuery, IEnumerable<ResponseDto>?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResponsesByTicketQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ResponseDto>?> Handle(GetResponsesByTicketQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null)
            return null;

        var responses = await _unitOfWork.Responses.GetByTicketIdAsync(request.TicketId);
        return responses.Select(r => r.ToDto());
    }
}

