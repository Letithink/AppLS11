using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Tickets.Commands;

public record DeleteTicketCommand(Guid Id) : IRequest<bool>;

internal sealed class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.Id);
        if (ticket == null)
            return false;

        _unitOfWork.Tickets.Remove(ticket);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

