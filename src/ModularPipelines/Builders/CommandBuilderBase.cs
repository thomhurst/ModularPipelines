using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Builders;

/// <summary>
/// Base implementation for command builders providing common execution options handling.
/// </summary>
/// <typeparam name="TBuilder">The builder self type used for fluent chaining.</typeparam>
/// <typeparam name="TOptions">The tool options type.</typeparam>
public abstract class CommandBuilderBase<TBuilder, TOptions> : ICommandBuilder<TBuilder, TOptions>
    where TBuilder : class, ICommandBuilder<TBuilder, TOptions>
    where TOptions : CommandLineToolOptions, new()
{
    private readonly ICommandContext _command;
    private TOptions _toolOptions;
    private CommandExecutionOptions _executionOptions = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="CommandBuilderBase{TBuilder, TOptions}"/> class.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    protected CommandBuilderBase(ICommandContext command)
    {
        _command = command;
        _toolOptions = new TOptions();
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="CommandBuilderBase{TBuilder, TOptions}"/> class.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    /// <param name="initialOptions">The initial tool options.</param>
    protected CommandBuilderBase(ICommandContext command, TOptions initialOptions)
    {
        _command = command;
        _toolOptions = initialOptions;
    }

    /// <summary>
    /// Gets the builder instance as the concrete type for fluent chaining.
    /// </summary>
    protected TBuilder Self => this as TBuilder
        ?? throw new InvalidOperationException($"{GetType().Name} must implement {typeof(TBuilder).Name}.");

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
            ?? [];
        vars[key] = value;
        _executionOptions = _executionOptions with { EnvironmentVariables = vars };
        return Self;
    }

    /// <inheritdoc />
    public TBuilder WithEnvironmentVariables(IDictionary<string, string?> variables)
    {
        var vars = _executionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? [];
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
