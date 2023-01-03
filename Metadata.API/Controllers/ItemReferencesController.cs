using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Extensions;
using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeWright.Metadata.API.Controllers;

/// <summary>
/// Item Reference API
/// </summary>
[ApiController]
[Route("api/items/references/v1")]
public class ItemReferencesController : ControllerBase
{
    /// <summary>
    /// Set all the references on an item, replacing any existing references
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The set references command</param>
    /// <returns>The command result</returns>
    [HttpPost]
    public Task<CommandResult> SetAsync([FromServices] ICommandHandler<ItemReferencesSetCommand> handler, ItemReferencesSetCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Add references for an item, ignoring any that already exist
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The add references command</param>
    /// <returns>The command result</returns>
    [HttpPost("add")]
    public Task<CommandResult> AddAsync([FromServices] ICommandHandler<ItemReferencesAddCommand> handler, ItemReferencesAddCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Remove the specified references from an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The remove references command</param>
    /// <returns>The command result</returns>
    [HttpPost("remove")]
    public Task<CommandResult> RemoveAsync([FromServices] ICommandHandler<ItemReferencesRemoveCommand> handler, ItemReferencesRemoveCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Fetch the references on an item
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>A list of references on the item</returns>
    [HttpGet("{tenantid}/{id}")]
    public Task<IEnumerable<ReferenceEntry>> GetByIdAsync([FromServices] IItemReferenceQuery query, string tenantId, string id)
        => query.FetchForIdAsync(id, tenantId);

    /// <summary>
    /// Search for items referencing this ID.
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The ID of the tenant to search</param>
    /// <param name="targetId">The item to match references for</param>
    /// <returns>A list of items matching the search criteria</returns>
    [HttpGet("referencing/{tenantid}/{targetId}")]
    public Task<IEnumerable<ItemResult>> GetReferencingAsync([FromServices] IItemReferenceQuery query, string tenantId, string targetId)
        => query.GetReferencingAsync(targetId, tenantId);
}
