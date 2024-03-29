﻿using System.ComponentModel.DataAnnotations;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to remove existing relationships between this item and others.
/// </summary>
public class ItemRelationshipsRemoveCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships to remove.
    /// </summary>
    [Required]
    public required IEnumerable<RelationshipEntry> Relationships { get; init; }
}
