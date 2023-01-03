using CodeWright.Common.Exceptions;
using FluentValidation;

namespace CodeWright.Common.EventSourcing
{
    public class ValidatingCommandHandler<TCommand, TCommandValidator> : ICommandHandler<TCommand>
        where TCommand : IDomainCommand, new() 
        where TCommandValidator : AbstractValidator<TCommand>, new()
    {
        private readonly ICommandHandler<TCommand> _wrappedHandler;

        public ValidatingCommandHandler(ICommandHandler<TCommand> wrappedHandler)
        {
            _wrappedHandler = wrappedHandler;
        }

        public async Task<CommandResult> HandleAsync(TCommand command, string userId)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var validator = new TCommandValidator();
            var result = validator.Validate(command);

            if (!result.IsValid)
                throw new BadRequestException(string.Join(',', result.Errors));

            return await _wrappedHandler.HandleAsync(command, userId);
        }
    }
}
