using System.ComponentModel.DataAnnotations;
using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Commands;
using CodeWright.Tagcat.API.Queries;
using CodeWright.Tagcat.API.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Codewright.Tagcat.API.Controllers;

/// <summary>
/// Item API
/// </summary>
[ApiController]
[Route("api/items/v1")]
public class ItemsController : ControllerBase
{
    /// <summary>
    /// Set all the metadata and relationships on an item, replacing any existing metadata and relationships
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The set relationships command</param>
    /// <returns>The command result</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> SetAsync(
        [FromServices] ICommandHandler<ItemSetAllCommand> handler,
        [FromBody] ItemSetAllCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Remove all the metadata and relationships on an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>The command result</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<CommandResult> DeketeAsync(
        [FromServices] ICommandHandler<ItemRemoveAllCommand> handler,
        [Required] string tenantId,
        [Required] string id)
        => handler.HandleAsync(new ItemRemoveAllCommand {TenantId = tenantId, Id = id }, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Fetch an item's metadata and relationships
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>The item details</returns>
    [HttpGet("{tenantId}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public Task<ItemResult> GetByIdAsync(
        [FromServices] IItemDetailQuery query, 
        string tenantId,
        string id)
        => query.GetByIdAsync(id, tenantId);

    /// <summary>
    /// Fetch an item's event audit trail
    /// </summary>
    /// <param name="eventStore">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <param name="fromVersion">The version to start from. Omit to start from the beginning.</param>
    /// <param name="limit">The maximum number of events to fetch</param>
    /// <returns>The item events</returns>
    [HttpGet("events/{tenantId}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<IEnumerable<IDomainEvent>> GetEventsByIdAsync(
        [FromServices] IEventStore eventStore,
        string tenantId,
        string id,
        [FromQuery] long? fromVersion = null,
        [FromQuery, Range(1, int.MaxValue)] int limit = 20)
        => eventStore.GetByIdAsync(id, tenantId, fromVersion ?? -1, limit);
}
