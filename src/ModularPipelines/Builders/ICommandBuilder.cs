using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Builders;

/// <summary>
/// Base interface for all command builders providing execution options.
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    /// Sets the working directory for command execution.
    /// </summary>
    /// <param name="directory">The working directory path.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithWorkingDirectory(string directory);

    /// <summary>
    /// Sets the execution timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets an environment variable for the command.
    /// </summary>
    /// <param name="key">The environment variable name.</param>
    /// <param name="value">The environment variable value.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithEnvironmentVariable(string key, string value);

    /// <summary>
    /// Sets multiple environment variables.
    /// </summary>
    /// <param name="variables">A dictionary of environment variables.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithEnvironmentVariables(IDictionary<string, string?> variables);

    /// <summary>
    /// Configures the command to run with sudo.
    /// </summary>
    /// <param name="sudo">Whether to use sudo. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithSudo(bool sudo = true);

    /// <summary>
    /// Configures whether to throw on non-zero exit code.
    /// </summary>
    /// <param name="throwOnError">Whether to throw on error. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithThrowOnError(bool throwOnError = true);

    /// <summary>
    /// Sets the graceful shutdown timeout.
    /// </summary>
    /// <param name="timeout">The graceful shutdown timeout duration.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithGracefulShutdownTimeout(TimeSpan timeout);

    /// <summary>
    /// Configures logging options for the command.
    /// </summary>
    /// <param name="options">The logging options.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithLogging(CommandLoggingOptions options);

    /// <summary>
    /// Configures logging options using a builder action.
    /// </summary>
    /// <param name="configure">An action to configure the logging options.</param>
    /// <returns>The builder instance for chaining.</returns>
    ICommandBuilder WithLogging(Action<CommandLoggingOptions> configure);
}

/// <summary>
/// Typed command builder with tool-specific options.
/// </summary>
/// <typeparam name="TBuilder">The concrete builder type for fluent chaining.</typeparam>
/// <typeparam name="TOptions">The tool options type.</typeparam>
public interface ICommandBuilder<TBuilder, TOptions> : ICommandBuilder
    where TBuilder : ICommandBuilder<TBuilder, TOptions>
    where TOptions : CommandLineToolOptions
{
    /// <summary>
    /// Executes the command with the configured options.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the command result.</returns>
    Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the built options without executing.
    /// Useful for inspection, testing, or hybrid usage patterns.
    /// </summary>
    /// <returns>A tuple containing the tool options and execution options.</returns>
    (TOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions();

    /// <summary>
    /// Sets the working directory for command execution.
    /// </summary>
    /// <param name="directory">The working directory path.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithWorkingDirectory(string directory);

    /// <summary>
    /// Sets the execution timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets an environment variable for the command.
    /// </summary>
    /// <param name="key">The environment variable name.</param>
    /// <param name="value">The environment variable value.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithEnvironmentVariable(string key, string value);

    /// <summary>
    /// Sets multiple environment variables.
    /// </summary>
    /// <param name="variables">A dictionary of environment variables.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithEnvironmentVariables(IDictionary<string, string?> variables);

    /// <summary>
    /// Configures the command to run with sudo.
    /// </summary>
    /// <param name="sudo">Whether to use sudo. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithSudo(bool sudo = true);

    /// <summary>
    /// Configures whether to throw on non-zero exit code.
    /// </summary>
    /// <param name="throwOnError">Whether to throw on error. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithThrowOnError(bool throwOnError = true);

    /// <summary>
    /// Sets the graceful shutdown timeout.
    /// </summary>
    /// <param name="timeout">The graceful shutdown timeout duration.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithGracefulShutdownTimeout(TimeSpan timeout);

    /// <summary>
    /// Configures logging options for the command.
    /// </summary>
    /// <param name="options">The logging options.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithLogging(CommandLoggingOptions options);

    /// <summary>
    /// Configures logging options using a builder action.
    /// </summary>
    /// <param name="configure">An action to configure the logging options.</param>
    /// <returns>The builder instance for chaining.</returns>
    new TBuilder WithLogging(Action<CommandLoggingOptions> configure);
}
