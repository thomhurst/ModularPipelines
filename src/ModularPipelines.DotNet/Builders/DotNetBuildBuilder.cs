using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using KeyValue = ModularPipelines.Models.KeyValue;

namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder implementation for dotnet build command.
/// </summary>
public class DotNetBuildBuilder : IDotNetBuildBuilder
{
    private readonly ICommand _command;
    private DotNetBuildOptions _toolOptions;
    private CommandExecutionOptions _executionOptions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetBuildBuilder"/> class.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    public DotNetBuildBuilder(ICommand command)
    {
        _command = command;
        _toolOptions = new DotNetBuildOptions();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetBuildBuilder"/> class with initial options.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    /// <param name="options">The initial build options.</param>
    public DotNetBuildBuilder(ICommand command, DotNetBuildOptions options)
    {
        _command = command;
        _toolOptions = options;
    }

    #region Tool-Specific Options

    /// <inheritdoc />
    public IDotNetBuildBuilder ForProject(string projectPath)
    {
        _toolOptions = _toolOptions with { ProjectSolution = projectPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithFramework(string framework)
    {
        _toolOptions = _toolOptions with { Framework = framework };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithConfiguration(string configuration)
    {
        _toolOptions = _toolOptions with { Configuration = configuration };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithRuntime(string runtime)
    {
        _toolOptions = _toolOptions with { Runtime = runtime };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithOutput(string outputPath)
    {
        _toolOptions = _toolOptions with { Output = outputPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithArtifactsPath(string artifactsPath)
    {
        _toolOptions = _toolOptions with { ArtifactsPath = artifactsPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithVersionSuffix(string versionSuffix)
    {
        _toolOptions = _toolOptions with { VersionSuffix = versionSuffix };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoRestore(bool noRestore = true)
    {
        _toolOptions = _toolOptions with { NoRestore = noRestore };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoIncremental(bool noIncremental = true)
    {
        _toolOptions = _toolOptions with { NoIncremental = noIncremental };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoDependencies(bool noDependencies = true)
    {
        _toolOptions = _toolOptions with { NoDependencies = noDependencies };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoLogo(bool noLogo = true)
    {
        _toolOptions = _toolOptions with { Nologo = noLogo };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithInteractive(bool interactive = true)
    {
        _toolOptions = _toolOptions with { Interactive = interactive };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithArch(string arch)
    {
        _toolOptions = _toolOptions with { Arch = arch };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithOs(string os)
    {
        _toolOptions = _toolOptions with { Os = os };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoSelfContained(bool noSelfContained = true)
    {
        _toolOptions = _toolOptions with { NoSelfContained = noSelfContained };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithDisableBuildServers(bool disableBuildServers = true)
    {
        _toolOptions = _toolOptions with { DisableBuildServers = disableBuildServers };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithDebug(bool debug = true)
    {
        _toolOptions = _toolOptions with { Debug = debug };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithProperty(string name, string value)
    {
        var properties = _toolOptions.Properties?.ToList() ?? [];
        properties.Add(new KeyValue(name, value));
        _toolOptions = _toolOptions with { Properties = properties };
        return this;
    }

    #endregion

    #region Execution Options

    /// <inheritdoc />
    public IDotNetBuildBuilder WithWorkingDirectory(string directory)
    {
        _executionOptions = _executionOptions with { WorkingDirectory = directory };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithTimeout(TimeSpan timeout)
    {
        _executionOptions = _executionOptions with { ExecutionTimeout = timeout };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithEnvironmentVariable(string key, string value)
    {
        var vars = _executionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        vars[key] = value;
        _executionOptions = _executionOptions with { EnvironmentVariables = vars };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithEnvironmentVariables(IDictionary<string, string?> variables)
    {
        var vars = _executionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        foreach (var kvp in variables)
        {
            vars[kvp.Key] = kvp.Value;
        }

        _executionOptions = _executionOptions with { EnvironmentVariables = vars };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithThrowOnError(bool throwOnError = true)
    {
        _executionOptions = _executionOptions with { ThrowOnNonZeroExitCode = throwOnError };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithLogging(CommandLoggingOptions options)
    {
        _executionOptions = _executionOptions with { LogSettings = options };
        return this;
    }

    #endregion

    #region Terminal Operations

    /// <inheritdoc />
    public virtual async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(_toolOptions, _executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public (DotNetBuildOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions()
    {
        return (_toolOptions, _executionOptions);
    }

    #endregion
}
