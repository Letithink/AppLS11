using AppLS10.api.Extensions;
using AppLS10.application.DTOs;
using AppLS10.application.UseCases.Responses.Commands;
using AppLS10.application.UseCases.Responses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppLS10.api.Controllers;

[ApiController]
[Route("api/ticket/{ticketId:guid}/response")]
[Authorize(Policy = "AdminOrUser")]
public class ResponseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResponseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid ticketId, CreateResponseDto dto)
    {
        var created = await _mediator.Send(new AddResponseCommand(ticketId, User.GetUserId(), dto));
        if (created == null)
            return NotFound();

        return Ok(created);
    }

    [HttpGet]
    public async Task<IActionResult> GetByTicket(Guid ticketId)
    {
        var responses = await _mediator.Send(new GetResponsesByTicketQuery(ticketId));
        if (responses == null)
            return NotFound();

        return Ok(responses);
    }
}
