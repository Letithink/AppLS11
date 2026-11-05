using AppLS10.application.DTOs;
using AppLS10.application.Mapping;
using AppLS10.domain.Entities;
using AppLS10.domain.Puertos;
using MediatR;

namespace AppLS10.application.UseCases.Responses.Commands;

public record AddResponseCommand(Guid TicketId, Guid ResponderId, CreateResponseDto Dto) : IRequest<ResponseDto?>;

internal sealed class AddResponseCommandHandler : IRequestHandler<AddResponseCommand, ResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddResponseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto?> Handle(AddResponseCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null)
            return null;

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = request.TicketId,
            ResponderId = request.ResponderId,
            Message = request.Dto.Message
        };

        await _unitOfWork.Responses.AddAsync(response);
        await _unitOfWork.SaveChangesAsync();

        return response.ToDto();
    }
}

