using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Base class for an asynchronous command with no settings.
    /// </summary>
    public abstract class AsyncCommand : ICommand<EmptyCommandSettings>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract Task<int> Execute(CommandContext context);

        Task<int> ICommand<EmptyCommandSettings>.Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return Execute(context);
        }

        Task<int> ICommand.Execute(CommandContext context, object settings)
        {
            return Execute(context);
        }

        ValidationResult ICommand.Validate(CommandContext context, object settings)
        {
            return ValidationResult.Success();
        }
    }
}