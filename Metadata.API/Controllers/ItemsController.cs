using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Extensions;
using CodeWright.Metadata.API.Queries;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Metadata.API.Controllers;

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
    public Task<CommandResult> SetAsync(
        [FromServices] ICommandHandler<ItemSetAllCommand> handler,
        ItemSetAllCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Remove all the metadata and relationships on an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>The command result</returns>
    [HttpDelete]
    public Task<CommandResult> DeketeAsync(
        [FromServices] ICommandHandler<ItemRemoveAllCommand> handler,
        string tenantId,
        string id)
        => handler.HandleAsync(new ItemRemoveAllCommand {TenantId = tenantId, Id = id }, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Fetch an item's metadata and relationships
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>The item details</returns>
    [HttpGet("{tenantId}/{id}")]
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
    public Task<IEnumerable<IDomainEvent>> GetEventsByIdAsync(
        [FromServices] IEventStore eventStore,
        string tenantId,
        string id,
        long? fromVersion = null,
        int limit = 20)
        => eventStore.GetByIdAsync(id, tenantId, fromVersion ?? -1, limit);
}
