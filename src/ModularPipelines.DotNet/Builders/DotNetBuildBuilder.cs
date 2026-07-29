using ModularPipelines.Builders;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.DotNet.Options;
using KeyValue = ModularPipelines.Models.KeyValue;

namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder implementation for dotnet build command.
/// </summary>
public class DotNetBuildBuilder : CommandBuilderBase<IDotNetBuildBuilder, DotNetBuildOptions>, IDotNetBuildBuilder
{
    /// <summary>
    /// Initialises a new instance of the <see cref="DotNetBuildBuilder"/> class.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    public DotNetBuildBuilder(ICommandContext command) : base(command)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DotNetBuildBuilder"/> class.
    /// </summary>
    /// <param name="command">The command interface for execution.</param>
    /// <param name="options">The initial build options.</param>
    public DotNetBuildBuilder(ICommandContext command, DotNetBuildOptions options) : base(command, options)
    {
    }

    #region Tool-Specific Options

    /// <inheritdoc />
    public IDotNetBuildBuilder ForProject(string projectPath)
    {
        ToolOptions = ToolOptions with { ProjectSolution = projectPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithFramework(string framework)
    {
        ToolOptions = ToolOptions with { Framework = framework };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithConfiguration(string configuration)
    {
        ToolOptions = ToolOptions with { Configuration = configuration };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithRuntime(string runtime)
    {
        ToolOptions = ToolOptions with { Runtime = runtime };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithOutput(string outputPath)
    {
        ToolOptions = ToolOptions with { Output = outputPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithArtifactsPath(string artifactsPath)
    {
        ToolOptions = ToolOptions with { ArtifactsPath = artifactsPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithVersionSuffix(string versionSuffix)
    {
        ToolOptions = ToolOptions with { VersionSuffix = versionSuffix };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoRestore(bool noRestore = true)
    {
        ToolOptions = ToolOptions with { NoRestore = noRestore };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoIncremental(bool noIncremental = true)
    {
        ToolOptions = ToolOptions with { NoIncremental = noIncremental };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoDependencies(bool noDependencies = true)
    {
        ToolOptions = ToolOptions with { NoDependencies = noDependencies };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoLogo(bool noLogo = true)
    {
        ToolOptions = ToolOptions with { NoLogo = noLogo };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithInteractive(bool interactive = true)
    {
        ToolOptions = ToolOptions with { Interactive = interactive };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithArch(string arch)
    {
        ToolOptions = ToolOptions with { Arch = arch };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithOs(string os)
    {
        ToolOptions = ToolOptions with { Os = os };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoSelfContained(bool noSelfContained = true)
    {
        ToolOptions = ToolOptions with { NoSelfContained = noSelfContained };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithDisableBuildServers(bool disableBuildServers = true)
    {
        ToolOptions = ToolOptions with { DisableBuildServers = disableBuildServers };
        return this;
    }

    /// <inheritdoc />
    [Obsolete("The dotnet --debug switch is no longer supported and this method has no effect.")]
    public IDotNetBuildBuilder WithDebug(bool debug = true)
    {
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithProperty(string name, string value)
    {
        var properties = ToolOptions.Properties?.ToList() ?? [];
        properties.Add(new KeyValue(name, value));
        ToolOptions = ToolOptions with { Properties = properties };
        return this;
    }

    #endregion
}
