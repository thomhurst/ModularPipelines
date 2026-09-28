namespace ModularPipelines.Context.Domains.Shell;

/// <summary>
/// Represents the remainder of the command execution pipeline: the next interceptor, or the
/// process executor after the last interceptor.
/// </summary>
/// <param name="invocation">The command invocation to execute.</param>
/// <param name="cancellationToken">The cancellation token.</param>
/// <returns>The command result.</returns>
public delegate ValueTask<CommandResult> CommandDelegate(
    CommandInvocation invocation,
    CancellationToken cancellationToken);

/// <summary>
/// Middleware that wraps command execution after the command has been parsed and before the process starts.
/// </summary>
/// <remarks>
/// <para>
/// Interceptors run in registration order: the first registered interceptor is the outermost and
/// receives the invocation first and the result last. Register interceptors with
/// <see cref="PipelineBuilderExtensions.AddCommandInterceptor{TInterceptor}(PipelineBuilder)"/>.
/// </para>
/// <para>
/// An interceptor can:
/// </para>
/// <list type="bullet">
/// <item>modify the command by passing a changed invocation to <c>next</c>, for example
/// <c>invocation with { CommandLine = ..., ExecutionOptions = ..., WorkingDirectory = ... }</c>;</item>
/// <item>observe or replace the result returned by <c>next</c>, or handle the exception it throws;</item>
/// <item>short-circuit by returning a result without calling <c>next</c>. The framework then applies
/// command metadata, logs the result, and throws <see cref="Exceptions.CommandException"/> for a nonzero
/// exit code when <see cref="Options.CommandExecutionOptions.ThrowOnNonZeroExitCode"/> is enabled.</item>
/// </list>
/// <para>
/// Only <see cref="CommandInvocation.CommandLine"/>, <see cref="CommandInvocation.ExecutionOptions"/>,
/// <see cref="CommandInvocation.WorkingDirectory"/>, and <see cref="CommandInvocation.ToolOptions"/> are applied.
/// <see cref="CommandInvocation.CommandInput"/> and <see cref="CommandInvocation.EnvironmentVariables"/> are
/// secret-masked views that the framework regenerates before each interceptor; edits to them are ignored.
/// Change environment variables through <see cref="Options.CommandExecutionOptions.EnvironmentVariables"/>.
/// The <see cref="Options.CommandExecutionOptions.ExecutionTimeout"/> starts before interception and cannot be changed by an interceptor.
/// </para>
/// </remarks>
public interface ICommandInterceptor
{
    /// <summary>
    /// Handles a command invocation.
    /// </summary>
    /// <param name="invocation">The parsed command invocation.</param>
    /// <param name="next">The rest of the pipeline. Call it to continue execution.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The command result.</returns>
    ValueTask<CommandResult> InvokeAsync(
        CommandInvocation invocation,
        CommandDelegate next,
        CancellationToken cancellationToken);
}
