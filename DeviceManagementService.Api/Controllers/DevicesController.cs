using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Application.Queries;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get device by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceById([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetDeviceByIdQuery(id);
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Device Not Found",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Get devices by optional filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeviceDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevicesByFilter(
            [FromQuery] string? name,
            [FromQuery] string? brand,
            [FromQuery] DeviceState? state,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetDevicesByFilterQuery(name, brand, state, page, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetDeviceById), new { id = result }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Detail = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                });
            }
        }

        /// <summary>
        /// Replace an existing device
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReplaceDevice([FromBody] ReplaceDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Device Not Found",
                    Detail = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Operation",
                    Detail = ex.Message
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Detail = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                });
            }
        }

        /// <summary>
        /// Update an existing device
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDevice([FromBody] UpdateDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Device Not Found",
                    Detail = ex.Message
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Detail = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                });
            }
        }

        /// <summary>
        /// Delete a device
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDevice([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteDeviceCommand(id);
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Device Not Found",
                    Detail = ex.Message
                });
            }
        }
    }
}
