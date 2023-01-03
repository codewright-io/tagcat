using CodeWright.Common.EventSourcing;
using FluentValidation;

namespace CodeWright.Metadata.API.Commands.Validation
{
    internal class ItemReferencesSetCommandValidator : AbstractValidator<ItemReferencesSetCommand>
    {
        public ItemReferencesSetCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleFor(command => command.TenantId).NotEmpty().MaximumLength(Identifiers.MaximumLength);
            RuleForEach(command => command.References)
                .Must(r => r.Type != Model.ReferenceType.Undefined).WithMessage("Reference Type cannot be undefined")
                .Must(r => !string.IsNullOrEmpty(r.TargetId)).WithMessage("Target ID must be supplied")
                .Must(r => r.TargetId.Length < Identifiers.MaximumLength).WithMessage("TargetId must be less than 40 characters");
        }
    }
}
