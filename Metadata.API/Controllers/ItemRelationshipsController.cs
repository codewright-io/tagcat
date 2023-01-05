using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Extensions;
using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeWright.Metadata.API.Controllers;

/// <summary>
/// Item Relationship API
/// </summary>
[ApiController]
[Route("api/items/relationships/v1")]
public class ItemRelationshipsController : ControllerBase
{
    /// <summary>
    /// Set all the relationships on an item, replacing any existing relationships
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The set relationships command</param>
    /// <returns>The command result</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> SetAsync(
        [FromServices] ICommandHandler<ItemRelationshipsSetCommand> handler,
        [FromBody] ItemRelationshipsSetCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Add relationships for an item, ignoring any that already exist
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The add relationships command</param>
    /// <returns>The command result</returns>
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> AddAsync(
        [FromServices] ICommandHandler<ItemRelationshipsAddCommand> handler,
        [FromBody] ItemRelationshipsAddCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Remove the specified relationships from an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The remove relationships command</param>
    /// <returns>The command result</returns>
    [HttpPost("remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> RemoveAsync(
        [FromServices] ICommandHandler<ItemRelationshipsRemoveCommand> handler,
        [FromBody] ItemRelationshipsRemoveCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Fetch the relationships on an item
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>A list of relationships on the item</returns>
    [HttpGet("{tenantId}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public Task<IEnumerable<RelationshipEntry>> GetByIdAsync(
        [FromServices] IItemRelationshipQuery query, 
        string tenantId, 
        string id)
        => query.FetchForIdAsync(id, tenantId);

    /// <summary>
    /// Search for items relationships this ID.
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The ID of the tenant to search</param>
    /// <param name="targetId">The item to match relationships for</param>
    /// <returns>A list of items matching the search criteria</returns>
    [HttpGet("referencing/{tenantId}/{targetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<ItemResult>> GetReferencingAsync(
        [FromServices] IItemRelationshipQuery query, 
        string tenantId, 
        string targetId)
        => query.GetReferencingAsync(targetId, tenantId);
}
