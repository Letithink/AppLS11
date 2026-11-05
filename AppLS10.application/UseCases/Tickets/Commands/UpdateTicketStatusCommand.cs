using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Constants;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Tickets.Commands;

public record UpdateTicketStatusCommand(Guid Id, UpdateTicketStatusDto Dto) : IRequest<TicketDto?>;

internal sealed class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, TicketDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto?> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.Id);
        if (ticket == null)
            return null;

        ticket.Status = request.Dto.Status.ToString();
        ticket.ClosedAt = request.Dto.Status == TicketStatus.cerrado
            ? DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            : null;

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync();

        return ticket.ToDto();
    }
}

