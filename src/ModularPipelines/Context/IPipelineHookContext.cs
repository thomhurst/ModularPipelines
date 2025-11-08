using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// The pipeline context, with access to configuration, DI and helper objects.
/// </summary>
/// <remarks>
/// This interface serves as a facade providing unified access to all pipeline capabilities.
/// Each property represents a focused helper for a specific domain (file system, commands, serialization, etc.).
///
/// The facade pattern is intentional here to:
/// - Provide a single entry point for all pipeline operations
/// - Allow dependency injection of all helpers
/// - Support extension methods for tool-specific integrations (Git, Docker, Azure, etc.)
///
/// Extension Pattern:
/// Tool integrations (ModularPipelines.Git, ModularPipelines.Docker, etc.) add extension methods
/// like context.Git() and context.Docker() that provide strongly-typed APIs for those tools.
/// </remarks>
public interface IPipelineHookContext
{
    #region DependencyInjection

    /// <summary>
    /// Gets the service provider orchestrating DI within the pipeline.
    /// </summary>
    /// <remarks>
    /// Use the generic Get&lt;T&gt;() method instead of accessing ServiceProvider directly
    /// when possible for better type safety.
    /// </remarks>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Helper method for retrieving services from the Service Provider.
    /// </summary>
    /// <typeparam name="T">Type to retrieve.</typeparam>
    /// <returns>The service instance, or null if not registered.</returns>
    public T? Get<T>();

    #endregion

    #region Configuration

    /// <summary>
    /// Gets the configuration powering the pipeline.
    /// </summary>
    /// <remarks>
    /// Configuration is loaded from appsettings.json, environment variables, user secrets, and command line.
    /// </remarks>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the pipeline's options.
    /// </summary>
    public IOptions<PipelineOptions> PipelineOptions { get; }

    #endregion

    #region Logging

    /// <summary>
    /// Gets the logger to be used from the pipeline.
    /// </summary>
    /// <remarks>
    /// This logger is module-scoped and will include the module name in log output.
    /// </remarks>
    public IModuleLogger Logger { get; }

    #endregion

    #region EnvironmentHelpers

    /// <summary>
    /// Gets helpers for getting information about the current environment.
    /// </summary>
    /// <remarks>
    /// Provides access to OS info, environment variables, working directory, etc.
    /// </remarks>
    public IEnvironmentContext Environment { get; }

    /// <summary>
    /// Gets helpers for detecting the current build system.
    /// </summary>
    /// <remarks>
    /// Detects GitHub Actions, Azure DevOps, Jenkins, GitLab CI, etc.
    /// </remarks>
    public IBuildSystemDetector BuildSystemDetector { get; }

    #endregion

    #region FileSystemHelpers

    /// <summary>
    /// Gets helpers for interacting with the filesystem.
    /// </summary>
    /// <remarks>
    /// Provides file/directory operations with logging and error handling.
    /// </remarks>
    public IFileSystemContext FileSystem { get; }

    /// <summary>
    /// Gets helpers for zipping and unzipping files and directories.
    /// </summary>
    public IZip Zip { get; }

    /// <summary>
    /// Gets helpers for checking the Checksum of a file.
    /// </summary>
    IChecksum Checksum { get; }

    #endregion

    #region CommandExecution

    /// <summary>
    /// Gets helpers for running commands.
    /// </summary>
    /// <remarks>
    /// Provides cross-platform command execution with output capture and logging.
    /// </remarks>
    public ICommand Command { get; }

    /// <summary>
    /// Gets helpers for running powershell.
    /// </summary>
    public IPowershell Powershell { get; }

    /// <summary>
    /// Gets helpers for executing bash commands.
    /// </summary>
    public IBash Bash { get; }

    #endregion

    #region Serialization

    /// <summary>
    /// Gets helpers for converting JSON.
    /// </summary>
    public IJson Json { get; }

    /// <summary>
    /// Gets helpers for convering XML.
    /// </summary>
    public IXml Xml { get; }

    /// <summary>
    /// Gets helpers for convering YAML.
    /// </summary>
    public IYaml Yaml { get; }

    #endregion

    #region Encoding

    /// <summary>
    /// Gets helpers for converting to and from hex strings.
    /// </summary>
    public IHex Hex { get; }

    /// <summary>
    /// Gets helpers for converting to and from Base64 strings.
    /// </summary>
    public IBase64 Base64 { get; }

    #endregion

    #region Security

    /// <summary>
    /// Gets helpers for hashing data.
    /// </summary>
    /// <remarks>
    /// Supports MD5, SHA1, SHA256, SHA512 hashing algorithms.
    /// </remarks>
    public IHasher Hasher { get; }

    #endregion

    #region NetworkHelpers

    /// <summary>
    /// Gets helpers for sending HTTP requests.
    /// </summary>
    /// <remarks>
    /// Provides a configured HttpClient with retry policies and logging.
    /// </remarks>
    public IHttp Http { get; }

    /// <summary>
    /// Gets helpers for downloading data from the web.
    /// </summary>
    /// <remarks>
    /// Includes progress tracking and verification helpers.
    /// </remarks>
    public IDownloader Downloader { get; }

    #endregion

    #region Installation

    /// <summary>
    /// Gets helpers for installing applications.
    /// </summary>
    /// <remarks>
    /// Supports package managers like Chocolatey, apt, brew, etc.
    /// </remarks>
    public IInstaller Installer { get; }

    #endregion

    #region Internal

    /// <summary>
    /// Gets the detector used to detect modules which depend on each other.
    /// </summary>
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }

    /// <summary>
    /// Gets the results repository used for storing module results.
    /// </summary>
    internal IModuleResultRepository ModuleResultRepository { get; }

    /// <summary>
    /// Used to initialise the logger for this context.
    /// </summary>
    /// <param name="getType">The module type.</param>
    internal void InitializeLogger(Type getType);

    /// <summary>
    /// Gets the cancellation token used for cancelling the pipeline on failures.
    /// </summary>
    internal EngineCancellationToken EngineCancellationToken { get; }

    #endregion
}