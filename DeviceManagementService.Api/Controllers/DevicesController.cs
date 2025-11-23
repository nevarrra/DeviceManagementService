using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Application.Queries;
using DeviceManagementService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController(IMediator mediator) : ControllerBase
    {
        private IMediator _mediator = mediator;

        /// <summary>
        /// Get device by ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetDeviceByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
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
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetDeviceById), new { id = result }, result);
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
            await _mediator.Send(command, cancellationToken);
            return NoContent();
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
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete a device
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDevice([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeleteDeviceCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
