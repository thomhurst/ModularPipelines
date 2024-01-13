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
public interface IPipelineHookContext
{
    /// <summary>
    /// Gets the service provider orchestrating DI within the pipeline.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the configuration powering the pipeline.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the pipeline's options.
    /// </summary>
    public IOptions<PipelineOptions> PipelineOptions { get; }

    /// <summary>
    /// Helper method for retrieving services from the Service Provider.
    /// </summary>
    /// <typeparam name="T">Type to retrieve.</typeparam>
    /// <returns>T.</returns>
    public T? Get<T>();

    /// <summary>
    /// Gets the logger to be used from the pipeline.
    /// </summary>
    public IModuleLogger Logger { get; }

    #region OutOfTheBoxHelpers

    /// <summary>
    /// Gets helpers for getting information about the current environment.
    /// </summary>
    public IEnvironmentContext Environment { get; }

    /// <summary>
    /// Gets helpers for interacting with the filesystem.
    /// </summary>
    public IFileSystemContext FileSystem { get; }

    /// <summary>
    /// Gets helpers for running commands.
    /// </summary>
    public ICommand Command { get; }

    /// <summary>
    /// Gets helpers for installing applications.
    /// </summary>
    public IInstaller Installer { get; }

    /// <summary>
    /// Gets helpers for zipping and unzipping files and directories.
    /// </summary>
    public IZip Zip { get; }

    /// <summary>
    /// Gets helpers for converting to and from hex strings.
    /// </summary>
    public IHex Hex { get; }

    /// <summary>
    /// Gets helpers for converting to and from Base64 strings.
    /// </summary>
    public IBase64 Base64 { get; }

    /// <summary>
    /// Gets helpers for hashing data.
    /// </summary>
    public IHasher Hasher { get; }

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

    /// <summary>
    /// Gets helpers for running powershell.
    /// </summary>
    public IPowershell Powershell { get; }

    /// <summary>
    /// Gets helpers for executing bash commands.
    /// </summary>
    public IBash Bash { get; }

    /// <summary>
    /// Gets helpers for detecting the current build system.
    /// </summary>
    public IBuildSystemDetector BuildSystemDetector { get; }

    /// <summary>
    /// Gets helpers for sending HTTP requests.
    /// </summary>
    public IHttp Http { get; }

    /// <summary>
    /// Gets helpers for downloading data from the web.
    /// </summary>
    public IDownloader Downloader { get; }

    /// <summary>
    /// Gets helpers for checking the Checksum of a file.
    /// </summary>
    IChecksum Checksum { get; }

    #endregion

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
}