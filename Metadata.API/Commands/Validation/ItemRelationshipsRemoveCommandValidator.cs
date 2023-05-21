﻿using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation;

internal class ItemRelationshipsRemoveCommandValidator : AbstractValidator<ItemRelationshipsRemoveCommand>
{
    public ItemRelationshipsRemoveCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
        RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
        RuleFor(command => command.Relationships).NotEmpty();
        RuleForEach(command => command.Relationships)
            .Must(r => r.Type != Model.RelationshipType.Undefined).WithMessage("Relationship Type cannot be undefined")
            .Must(r => !string.IsNullOrEmpty(r.TargetId)).WithMessage("Target ID must be supplied")
            .Must(r => r.TargetId.Length < Identifiers.MaximumLength).WithMessage("TargetId must be less than 40 characters");
    }
}
