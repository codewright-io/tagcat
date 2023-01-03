using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation
{
    internal class ItemSetAllCommandValidator : AbstractValidator<ItemSetAllCommand>
    {
        public ItemSetAllCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleForEach(command => command.Metadata)
                .Must(r => !string.IsNullOrEmpty(r.Name)).WithMessage("Metadata name must be supplied")
                .Must(r => r.Name.Length < Identifiers.MaximumLength).WithMessage("Name must be less than 40 characters")
                .Must(r => r.Value.Length < 100).WithMessage("Value must be less than 100 characters");
            RuleForEach(command => command.Relationships)
                .Must(r => r.Type != Model.RelationshipType.Undefined).WithMessage("Relationship Type cannot be undefined")
                .Must(r => !string.IsNullOrEmpty(r.TargetId)).WithMessage("Target ID must be supplied")
                .Must(r => r.TargetId.Length < Identifiers.MaximumLength).WithMessage("TargetId must be less than 40 characters");
        }
    }
}
