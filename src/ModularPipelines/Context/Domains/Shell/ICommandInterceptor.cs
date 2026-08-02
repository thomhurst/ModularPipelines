using ModularPipelines.Models;

namespace ModularPipelines.Context.Domains.Shell;

/// <summary>
/// Intercepts commands after parsing and before process creation.
/// </summary>
public interface ICommandInterceptor
{
    /// <summary>
    /// Attempts to handle a command.
    /// </summary>
    /// <param name="invocation">The parsed command invocation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A replacement result when the command was handled; otherwise, <see langword="null"/>
    /// to continue to the next interceptor or execute the process.
    /// </returns>
    ValueTask<CommandResult?> InterceptAsync(
        CommandInvocation invocation,
        CancellationToken cancellationToken = default);
}
