using ModularPipelines.Builders;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder interface for dotnet build command.
/// Provides a discoverable, chainable API for configuring build options.
/// </summary>
public interface IDotNetBuildBuilder : ICommandBuilder<IDotNetBuildBuilder, DotNetBuildOptions>
{
    #region Tool-Specific Options

    /// <summary>
    /// Sets the project or solution file to build.
    /// </summary>
    /// <param name="projectPath">The path to the project or solution file.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder ForProject(string projectPath);

    /// <summary>
    /// Sets the target framework to build for.
    /// </summary>
    /// <param name="framework">The target framework (e.g., "net8.0", "net9.0").</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithFramework(string framework);

    /// <summary>
    /// Sets the build configuration.
    /// </summary>
    /// <param name="configuration">The configuration (e.g., "Debug", "Release").</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithConfiguration(string configuration);

    /// <summary>
    /// Sets the target runtime.
    /// </summary>
    /// <param name="runtime">The runtime identifier (e.g., "win-x64", "linux-x64").</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithRuntime(string runtime);

    /// <summary>
    /// Sets the output directory for build artifacts.
    /// </summary>
    /// <param name="outputPath">The output directory path.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithOutput(string outputPath);

    /// <summary>
    /// Sets the artifacts path for all output.
    /// </summary>
    /// <param name="artifactsPath">The artifacts directory path.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithArtifactsPath(string artifactsPath);

    /// <summary>
    /// Sets the version suffix for the build.
    /// </summary>
    /// <param name="versionSuffix">The version suffix.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithVersionSuffix(string versionSuffix);

    /// <summary>
    /// Disables restore before build.
    /// </summary>
    /// <param name="noRestore">Whether to skip restore. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithNoRestore(bool noRestore = true);

    /// <summary>
    /// Disables incremental building.
    /// </summary>
    /// <param name="noIncremental">Whether to disable incremental build. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithNoIncremental(bool noIncremental = true);

    /// <summary>
    /// Disables building project-to-project references.
    /// </summary>
    /// <param name="noDependencies">Whether to skip building dependencies. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithNoDependencies(bool noDependencies = true);

    /// <summary>
    /// Disables the startup banner and copyright message.
    /// </summary>
    /// <param name="noLogo">Whether to suppress the logo. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithNoLogo(bool noLogo = true);

    /// <summary>
    /// Enables interactive mode for authentication prompts.
    /// </summary>
    /// <param name="interactive">Whether to enable interactive mode. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithInteractive(bool interactive = true);

    /// <summary>
    /// Sets the target architecture.
    /// </summary>
    /// <param name="arch">The architecture (e.g., "x64", "arm64").</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithArch(string arch);

    /// <summary>
    /// Sets the target operating system.
    /// </summary>
    /// <param name="os">The operating system (e.g., "win", "linux", "osx").</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithOs(string os);

    /// <summary>
    /// Publishes as framework-dependent (not self-contained).
    /// </summary>
    /// <param name="noSelfContained">Whether to disable self-contained. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithNoSelfContained(bool noSelfContained = true);

    /// <summary>
    /// Disables build servers.
    /// </summary>
    /// <param name="disableBuildServers">Whether to disable build servers. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithDisableBuildServers(bool disableBuildServers = true);

    /// <summary>
    /// Retained for compatibility. The dotnet CLI no longer supports the debug switch.
    /// </summary>
    /// <param name="debug">Ignored.</param>
    /// <returns>The builder instance for chaining.</returns>
    [Obsolete("The dotnet --debug switch is no longer supported and this method has no effect.")]
    IDotNetBuildBuilder WithDebug(bool debug = true);

    /// <summary>
    /// Adds an MSBuild property.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The property value.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithProperty(string name, string value);

    #endregion
}
