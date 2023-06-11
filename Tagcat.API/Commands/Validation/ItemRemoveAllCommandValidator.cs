using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation;

internal class ItemRemoveAllCommandValidator : AbstractValidator<ItemRemoveAllCommand>
{
    public ItemRemoveAllCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
        RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
    }
}
