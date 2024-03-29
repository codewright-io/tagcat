using System.ComponentModel.DataAnnotations;
using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Commands;
using CodeWright.Tagcat.API.Model;
using CodeWright.Tagcat.API.Queries;
using CodeWright.Tagcat.API.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;

namespace CodeWright.Tagcat.API.Controllers;

/// <summary>
/// Item Metadata API
/// </summary>
[ApiController]
[Route("api/items/metadata/v1")]
public class ItemMetadataController : ControllerBase
{
    /// <summary>
    /// Set all the metadata on an item, replacing any existing metadata
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The set metadata command</param>
    /// <returns>The command result</returns>
    [HttpPost(Name = "SetMetadata")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> SetAsync(
        [FromServices] ICommandHandler<ItemMetadataSetCommand> handler,
        [FromBody] ItemMetadataSetCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Add metadata to an item, ignoring any that already exist
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The add metadata command</param>
    /// <returns>The command result</returns>
    [HttpPost("add", Name = "AddMetadata")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<CommandResult> AddAsync(
        [FromServices] ICommandHandler<ItemMetadataAddCommand> handler,
        [FromBody] ItemMetadataAddCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Remove the specified metadata from an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The remove metadata command</param>
    /// <returns>The command result</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [HttpPost("remove", Name = "RemoveMetadata")]
    public Task<CommandResult> RemoveAsync(
        [FromServices] ICommandHandler<ItemMetadataRemoveCommand> handler,
        [FromBody] ItemMetadataRemoveCommand command)
        => handler.HandleAsync(command, HttpContext.GetUserId() ?? "");

    /// <summary>
    /// Fetch the metadata on an item
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>A list of metadata on the item</returns>
    [HttpGet("{tenantId}/{id}", Name = "GetMetadataById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<MetadataEntry>> GetByIdAsync(
        [FromServices] IItemMetadataQuery query, 
        string tenantId, 
        string id)
        => query.FetchForIdAsync(id, tenantId);

    /// <summary>
    /// Search for items with the specified metadata name and values.
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="tenantId">The ID of the tenant to search</param>
    /// <param name="name">The metadata name to match</param>
    /// <param name="value">The metadata value to match</param>
    /// <param name="secondaryName">An optional secondary name to match</param>
    /// <param name="secondaryValue">An optional secondary value to match</param>
    /// <param name="limit">The maximum number of results to return</param>
    /// <param name="offset">An offset used to paginate results</param>
    /// <returns>A list of items matching the search criteria</returns>
    [HttpGet("search/{tenantId}", Name = "SeachMetadata")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<ItemResult>> SearchAsync(
        [FromServices] IItemMetadataQuery query,
        string tenantId,
        [FromQuery, Required] string name,
        [FromQuery, Required] string value,
        [FromQuery] string? secondaryName = null,
        [FromQuery] string? secondaryValue = null,
        [FromQuery, Range(1, int.MaxValue)] int limit = 20,
        [FromQuery, Range(0, int.MaxValue)] int offset = 0)
        => query.GetItemsByMetadataAsync(tenantId, name, value, secondaryName, secondaryValue, limit, offset);
}
