﻿using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Tagcat.API.Commands.Validation;

internal class ItemTagsSetCommandValidator : AbstractValidator<ItemTagsSetCommand>
{
    public ItemTagsSetCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
        RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
        RuleForEach(command => command.Tags)
            .Must(r => !string.IsNullOrEmpty(r)).WithMessage("Tag must be supplied")
            .Must(r => r.Length < 30).WithMessage("Tag must be less than 30 characters");
    }
}
