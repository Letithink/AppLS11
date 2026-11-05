using AppLS10.api.Extensions;
using AppLS10.application.DTOs;
using AppLS10.application.UseCases.Tickets.Commands;
using AppLS10.application.UseCases.Tickets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppLS10.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "AdminOrUser")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketDto dto)
    {
        var ticket = await _mediator.Send(new CreateTicketCommand(User.GetUserId(), dto));
        return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetTicketsQuery()));
    }

    [Authorize(Policy = "AdminOrUser")]
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        return Ok(await _mediator.Send(new GetMyTicketsQuery(User.GetUserId())));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _mediator.Send(new GetTicketByIdQuery(id));
        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateTicketStatusDto dto)
    {
        var ticket = await _mediator.Send(new UpdateTicketStatusCommand(id, dto));
        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _mediator.Send(new DeleteTicketCommand(id));
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
