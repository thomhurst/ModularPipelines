using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder interface for dotnet build command.
/// Provides a discoverable, chainable API for configuring build options.
/// </summary>
public interface IDotNetBuildBuilder
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
    /// Enables debug mode.
    /// </summary>
    /// <param name="debug">Whether to enable debug mode. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithDebug(bool debug = true);

    /// <summary>
    /// Adds an MSBuild property.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The property value.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithProperty(string name, string value);

    #endregion

    #region Execution Options

    /// <summary>
    /// Sets the working directory for command execution.
    /// </summary>
    /// <param name="directory">The working directory path.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithWorkingDirectory(string directory);

    /// <summary>
    /// Sets the execution timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets an environment variable for the command.
    /// </summary>
    /// <param name="key">The environment variable name.</param>
    /// <param name="value">The environment variable value.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithEnvironmentVariable(string key, string value);

    /// <summary>
    /// Sets multiple environment variables.
    /// </summary>
    /// <param name="variables">A dictionary of environment variables.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithEnvironmentVariables(IDictionary<string, string?> variables);

    /// <summary>
    /// Configures whether to throw on non-zero exit code.
    /// </summary>
    /// <param name="throwOnError">Whether to throw on error. Defaults to true.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithThrowOnError(bool throwOnError = true);

    /// <summary>
    /// Configures logging options for the command.
    /// </summary>
    /// <param name="options">The logging options.</param>
    /// <returns>The builder instance for chaining.</returns>
    IDotNetBuildBuilder WithLogging(CommandLoggingOptions options);

    #endregion

    #region Terminal Operations

    /// <summary>
    /// Executes the build command with the configured options.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the command result.</returns>
    Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the built options without executing.
    /// Useful for inspection, testing, or hybrid usage patterns.
    /// </summary>
    /// <returns>A tuple containing the tool options and execution options.</returns>
    (DotNetBuildOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions();

    #endregion
}
