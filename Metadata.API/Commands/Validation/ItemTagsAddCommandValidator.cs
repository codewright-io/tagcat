using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation
{
    internal class ItemTagsAddCommandValidator : AbstractValidator<ItemTagsAddCommand>
    {
        public ItemTagsAddCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleFor(command => command.Tags).NotEmpty();
            RuleForEach(command => command.Tags)
                .Must(r => !string.IsNullOrEmpty(r)).WithMessage("Tag must be supplied")
                .Must(r => r.Length < 30).WithMessage("Tag must be less than 30 characters");
        }
    }
}
