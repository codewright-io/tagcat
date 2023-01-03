using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation
{
    internal class ItemMetadataSetCommandValidator : AbstractValidator<ItemMetadataSetCommand>
    {
        public ItemMetadataSetCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleForEach(command => command.Metadata)
                .Must(r => !string.IsNullOrEmpty(r.Name)).WithMessage("Metadata name must be supplied")
                .Must(r => r.Name.Length < Identifiers.MaximumLength).WithMessage("Name must be less than 40 characters")
                .Must(r => r.Value.Length < 100).WithMessage("Value must be less than 100 characters");
        }
    }
}
