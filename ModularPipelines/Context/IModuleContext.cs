using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IModuleContext
{
    internal EngineCancellationToken EngineCancellationToken { get; }
    internal TModule? GetModule<TModule>() where TModule : ModuleBase;
    internal ModuleBase? GetModule(Type type);
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }
    public IOptions<PipelineOptions> PipelineOptions { get; }
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }
    internal IModuleResultRepository ModuleResultRepository { get; }
    internal void FetchLogger(Type getType);
    public T? Get<T>();
    public ILogger Logger { get; }

    #region OutOfTheBoxHelpers

    public IEnvironmentContext Environment { get; }
    public IFileSystemContext FileSystem { get; }
    public ICommand Command { get; }
    public IInstaller Installer { get; }
    public IZip Zip { get; }
    public IHex Hex { get; }
    public IBase64 Base64 { get; }
    public IHasher Hasher { get; }
    public IJson Json { get; }
    public IXml Xml { get; }
    public IPowershell Powershell { get; }

    #endregion
}
