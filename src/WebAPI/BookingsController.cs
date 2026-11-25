using EventFlow.Application;
using EventFlow.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EventFlow.WebAPI.Controllers;

[ApiController]
[Route("v1/bookings")]
public sealed class BookingsController : ControllerBase
{
    private readonly CreateBookingUseCase _useCase;

    public BookingsController(CreateBookingUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookingResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateBookingCommand(
                request.EventId, request.CustomerId, request.SeatIds);

            var result = await _useCase.ExecuteAsync(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (SeatUnavailableException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
}

public sealed record CreateBookingRequest(
    Guid EventId,
    Guid CustomerId,
    IReadOnlyCollection<Guid> SeatIds);
