using System.Globalization;
using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Extensions;
using CodeWright.Metadata.API.Queries.Interfaces;
using CodeWright.Metadata.API.Queries.Views;
using Microsoft.AspNetCore.Mvc;

namespace CodeWright.Metadata.API.Controllers;

/// <summary>
/// Item Tag API
/// </summary>
[ApiController]
[Route("api/items/tags/v1")]
public class ItemTagsController : ControllerBase
{
    /// <summary>
    /// Set all the tags on an item, replacing any existing tags
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="command">The set tags command</param>
    /// <param name="contextAccessor">The HTTP context accessor</param>
    /// <returns>The command result</returns>
    [HttpPost]
    public Task<CommandResult> SetAsync(
        [FromServices] ICommandHandler<ItemTagsSetCommand> handler,
        [FromServices] IHttpContextAccessor contextAccessor,
        [FromBody] ItemTagsSetCommand command)
    {
        command.Culture = GetCleanCulture(contextAccessor, command.Culture);
        return handler.HandleAsync(command, HttpContext.GetUserId() ?? "");
    }

    /// <summary>
    /// Add tags for an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="contextAccessor">The HTTP context accessor</param>
    /// <param name="command">The add tags command</param>
    /// <returns>The command result</returns>
    [HttpPost("add")]
    public Task<CommandResult> AddAsync(
        [FromServices] ICommandHandler<ItemTagsAddCommand> handler,
        [FromServices] IHttpContextAccessor contextAccessor,
        [FromBody] ItemTagsAddCommand command)
    {
        command.Culture = GetCleanCulture(contextAccessor, command.Culture);
        return handler.HandleAsync(command, HttpContext.GetUserId() ?? "");
    }

    /// <summary>
    /// Remove the specified tags from an item
    /// </summary>
    /// <param name="handler">The command handler</param>
    /// <param name="contextAccessor">The HTTP context accessor</param>
    /// <param name="command">The remove tags command</param>
    /// <returns>The command result</returns>
    [HttpPost("remove")]
    public Task<CommandResult> RemoveAsync(
        [FromServices] ICommandHandler<ItemTagsRemoveCommand> handler,
        [FromServices] IHttpContextAccessor contextAccessor,
        [FromBody] ItemTagsRemoveCommand command)
    {
        command.Culture = GetCleanCulture(contextAccessor, command.Culture);
        return handler.HandleAsync(command, HttpContext.GetUserId() ?? "");
    }

    /// <summary>
    /// Fetch the tags on an item
    /// </summary>
    /// <param name="query">The query handler</param>
    /// <param name="contextAccessor">The HTTP context accessor</param>
    /// <param name="culture">An optional two letter ISO culture to fetch the tags for</param>
    /// <param name="tenantId">The tenant ID for the item</param>
    /// <param name="id">The ID of the item</param>
    /// <returns>A list of relationships on the item</returns>
    [HttpGet("{tenantId}/{id}")]
    public Task<IEnumerable<ItemTagViewEntry>> GetByIdAsync(
        [FromServices] IItemTagQuery query,
        [FromServices] IHttpContextAccessor contextAccessor,
        [FromQuery] string? culture,
        string tenantId, 
        string id)
        => query.FetchForIdAsync(id, tenantId, GetCleanCulture(contextAccessor, culture));

    /// <summary>
    /// Get two letter ISO culture based on the following priorities:
    /// 1. From the command
    /// 2. From the accept-language header
    /// 3. From the default OS culture
    /// </summary>
    private static string GetCleanCulture(IHttpContextAccessor contextAccessor, string? culture)
    {
        // Use the explicitly specified culture first, then from the header, otherwise default
        var cultureInfo = !string.IsNullOrEmpty(culture) ? new CultureInfo(culture) :
            contextAccessor.HttpContext?.GetCulture() ?? CultureInfo.CurrentCulture;

        // Simplify to just use neutral language
        return cultureInfo.TwoLetterISOLanguageName;
    }
}
