using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

[ExcludeFromCodeCoverage]
internal class NoModulePipelineContext : IPipelineContext
{
    private readonly IPipelineContext _pipelineContextImplementation;

    public NoModulePipelineContext(IPipelineContext pipelineContextImplementation)
    {
        _pipelineContextImplementation = pipelineContextImplementation;
    }

    public EngineCancellationToken EngineCancellationToken => _pipelineContextImplementation.EngineCancellationToken;

    public TModule GetModule<TModule>()
        where TModule : ModuleBase
    {
        throw new PipelineException("Getting a module is not supported outside of another module.");
    }
    
    public ModuleBase GetModule(Type type)
    {
        throw new PipelineException("Getting a module is not supported outside of another module.");
    }

    public IServiceProvider ServiceProvider => _pipelineContextImplementation.ServiceProvider;

    public IConfiguration Configuration => _pipelineContextImplementation.Configuration;

    public IOptions<PipelineOptions> PipelineOptions => _pipelineContextImplementation.PipelineOptions;

    public IDependencyCollisionDetector DependencyCollisionDetector => _pipelineContextImplementation.DependencyCollisionDetector;

    public IModuleResultRepository ModuleResultRepository => _pipelineContextImplementation.ModuleResultRepository;

    public void FetchLogger(Type getType)
    {
        _pipelineContextImplementation.FetchLogger(getType);
    }

    public T? Get<T>()
    {
        return _pipelineContextImplementation.Get<T>();
    }

    public IModuleLogger Logger => _pipelineContextImplementation.Logger;

    public IEnvironmentContext Environment => _pipelineContextImplementation.Environment;

    public IFileSystemContext FileSystem => _pipelineContextImplementation.FileSystem;

    public ICommand Command => _pipelineContextImplementation.Command;

    public IInstaller Installer => _pipelineContextImplementation.Installer;

    public IZip Zip => _pipelineContextImplementation.Zip;

    public IHex Hex => _pipelineContextImplementation.Hex;

    public IBase64 Base64 => _pipelineContextImplementation.Base64;

    public IHasher Hasher => _pipelineContextImplementation.Hasher;

    public IJson Json => _pipelineContextImplementation.Json;

    public IXml Xml => _pipelineContextImplementation.Xml;

    public IPowershell Powershell => _pipelineContextImplementation.Powershell;

    public IBash Bash => _pipelineContextImplementation.Bash;

    public IBuildSystemDetector BuildSystemDetector => _pipelineContextImplementation.BuildSystemDetector;

    public IHttp Http => _pipelineContextImplementation.Http;

    public IDownloader Downloader => _pipelineContextImplementation.Downloader;

    public IChecksum Checksum => _pipelineContextImplementation.Checksum;
}