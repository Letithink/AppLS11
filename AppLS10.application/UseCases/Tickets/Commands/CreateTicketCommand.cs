using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Constants;
using AppLS10.domain.Entities;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Tickets.Commands;

public record CreateTicketCommand(Guid UserId, CreateTicketDto Dto) : IRequest<TicketDto>;

internal sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            Status = TicketStatus.abierto.ToString()
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        return ticket.ToDto();
    }
}

