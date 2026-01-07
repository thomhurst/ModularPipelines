using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Builders;

/// <summary>
/// Base implementation for command builders providing common execution options handling.
/// </summary>
/// <typeparam name="TBuilder">The concrete builder type for fluent chaining.</typeparam>
/// <typeparam name="TOptions">The tool options type.</typeparam>
public abstract class CommandBuilderBase<TBuilder, TOptions> : ICommandBuilder<TBuilder, TOptions>
    where TBuilder : CommandBuilderBase<TBuilder, TOptions>
    where TOptions : CommandLineToolOptions, new()
{
    private readonly ICommand _command;
    private TOptions _toolOptions;
    private CommandExecutionOptions _executionOptions = new();

    /// <summary>
    /// Initializes a new instance of the builder with default options.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    protected CommandBuilderBase(ICommand command)
    {
        _command = command;
        _toolOptions = new TOptions();
    }

    /// <summary>
    /// Initializes a new instance of the builder with initial options.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    /// <param name="initialOptions">The initial tool options.</param>
    protected CommandBuilderBase(ICommand command, TOptions initialOptions)
    {
        _command = command;
        _toolOptions = initialOptions;
    }

    /// <summary>
    /// Gets the builder instance as the concrete type for fluent chaining.
    /// </summary>
    protected TBuilder Self => (TBuilder)this;

    /// <summary>
    /// Gets or sets the tool-specific options being built.
    /// </summary>
    protected TOptions ToolOptions
    {
        get => _toolOptions;
        set => _toolOptions = value;
    }

    /// <summary>
    /// Gets or sets the execution options being built.
    /// </summary>
    protected CommandExecutionOptions ExecutionOptions
    {
        get => _executionOptions;
        set => _executionOptions = value;
    }

    /// <inheritdoc />
    public TBuilder WithWorkingDirectory(string directory)
    {
        _executionOptions = _executionOptions with { WorkingDirectory = directory };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithTimeout(TimeSpan timeout)
    {
        _executionOptions = _executionOptions with { ExecutionTimeout = timeout };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithEnvironmentVariable(string key, string value)
    {
        var vars = _executionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        vars[key] = value;
        _executionOptions = _executionOptions with { EnvironmentVariables = vars };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithEnvironmentVariables(IDictionary<string, string?> variables)
    {
        var vars = _executionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        foreach (var kvp in variables)
        {
            vars[kvp.Key] = kvp.Value;
        }

        _executionOptions = _executionOptions with { EnvironmentVariables = vars };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithSudo(bool sudo = true)
    {
        _executionOptions = _executionOptions with { Sudo = sudo };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithThrowOnError(bool throwOnError = true)
    {
        _executionOptions = _executionOptions with { ThrowOnNonZeroExitCode = throwOnError };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithGracefulShutdownTimeout(TimeSpan timeout)
    {
        _executionOptions = _executionOptions with { GracefulShutdownTimeout = timeout };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithLogging(CommandLoggingOptions options)
    {
        _executionOptions = _executionOptions with { LogSettings = options };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithLogging(Action<CommandLoggingOptions> configure)
    {
        var options = new CommandLoggingOptions();
        configure(options);
        return WithLogging(options);
    }

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithWorkingDirectory(string directory) => WithWorkingDirectory(directory);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithTimeout(TimeSpan timeout) => WithTimeout(timeout);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithEnvironmentVariable(string key, string value) => WithEnvironmentVariable(key, value);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithEnvironmentVariables(IDictionary<string, string?> variables) => WithEnvironmentVariables(variables);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithSudo(bool sudo) => WithSudo(sudo);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithThrowOnError(bool throwOnError) => WithThrowOnError(throwOnError);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithGracefulShutdownTimeout(TimeSpan timeout) => WithGracefulShutdownTimeout(timeout);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithLogging(CommandLoggingOptions options) => WithLogging(options);

    /// <inheritdoc />
    ICommandBuilder ICommandBuilder.WithLogging(Action<CommandLoggingOptions> configure) => WithLogging(configure);

    /// <inheritdoc />
    public virtual async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(_toolOptions, _executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public (TOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions()
    {
        return (_toolOptions, _executionOptions);
    }
}
