using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Module-specific context that wraps the pipeline context and adds module-specific capabilities.
/// </summary>
internal class ModuleContext : IModuleContext
{
    private readonly IPipelineContext _pipelineContext;
    private readonly IModule _currentModule;
    private readonly ModuleExecutionContext _executionContext;
    private readonly IModuleLogger _logger;

    public ModuleContext(
        IPipelineContext pipelineContext,
        IModule currentModule,
        ModuleExecutionContext executionContext,
        IModuleLogger logger)
    {
        _pipelineContext = pipelineContext;
        _currentModule = currentModule;
        _executionContext = executionContext;
        _logger = logger;
    }

    #region IModuleContext specific methods

    public ModuleResult<TResult> GetModule<TModule, TResult>()
        where TModule : Module<TResult>
    {
        var moduleType = typeof(TModule);

        // Check for self-reference
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleReferencingSelfException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        var result = GetModuleIfRegistered<TModule, TResult>();

        if (result == null)
        {
            throw new ModuleNotRegisteredException(
                $"Module '{moduleType.Name}' is not registered in the pipeline.\n" +
                $"Add '.AddModule<{moduleType.Name}>()' to your pipeline configuration.",
                null);
        }

        return result;
    }

    public ModuleResult<TResult>? GetModuleIfRegistered<TModule, TResult>()
        where TModule : Module<TResult>
    {
        var moduleType = typeof(TModule);

        // Find it via the module registry
        var registry = ServiceProvider.GetService<IModuleResultRegistry>();
        if (registry != null)
        {
            return registry.GetResult<TResult>(moduleType);
        }

        return null;
    }

    public async Task<T> SubModule<T>(string name, Func<Task<T>> action)
    {
        var tracker = new SubModuleTracker(name, _currentModule.GetType());
        _executionContext.SubModules.Add(tracker);
        return await tracker.ExecuteAsync(action).ConfigureAwait(false);
    }

    public async Task SubModule(string name, Func<Task> action)
    {
        var tracker = new SubModuleTracker(name, _currentModule.GetType());
        _executionContext.SubModules.Add(tracker);
        await tracker.ExecuteAsync(action).ConfigureAwait(false);
    }

    #endregion

    #region IPipelineContext delegation

    TModule? IPipelineContext.GetModule<TModule>()
        where TModule : class
    {
        var moduleType = typeof(TModule);

        // Check for self-reference
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleReferencingSelfException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        return _pipelineContext.GetModule<TModule>();
    }

    IModule? IPipelineContext.GetModule(Type type)
    {
        return _pipelineContext.GetModule(type);
    }

    #endregion

    #region IPipelineHookContext delegation

    public IServiceProvider ServiceProvider => _pipelineContext.ServiceProvider;

    public T? Get<T>() => _pipelineContext.Get<T>();

    public IConfiguration Configuration => _pipelineContext.Configuration;

    public IOptions<PipelineOptions> PipelineOptions => _pipelineContext.PipelineOptions;

    public IModuleLogger Logger => _logger;

    public IEnvironmentContext Environment => _pipelineContext.Environment;

    public IBuildSystemDetector BuildSystemDetector => _pipelineContext.BuildSystemDetector;

    public IFileSystemContext FileSystem => _pipelineContext.FileSystem;

    public IZip Zip => _pipelineContext.Zip;

    public IChecksum Checksum => _pipelineContext.Checksum;

    public ICommand Command => _pipelineContext.Command;

    public IPowershell Powershell => _pipelineContext.Powershell;

    public IBash Bash => _pipelineContext.Bash;

    public IJson Json => _pipelineContext.Json;

    public IXml Xml => _pipelineContext.Xml;

    public IYaml Yaml => _pipelineContext.Yaml;

    public IHex Hex => _pipelineContext.Hex;

    public IBase64 Base64 => _pipelineContext.Base64;

    public IHasher Hasher => _pipelineContext.Hasher;

    public IHttp Http => _pipelineContext.Http;

    public IDownloader Downloader => _pipelineContext.Downloader;

    public IInstaller Installer => _pipelineContext.Installer;

    IDependencyCollisionDetector IPipelineHookContext.DependencyCollisionDetector =>
        _pipelineContext.DependencyCollisionDetector;

    IModuleResultRepository IPipelineHookContext.ModuleResultRepository =>
        _pipelineContext.ModuleResultRepository;

    void IPipelineHookContext.InitializeLogger(Type getType)
    {
        // Logger is already initialized in constructor
    }

    EngineCancellationToken IPipelineHookContext.EngineCancellationToken =>
        _pipelineContext.EngineCancellationToken;

    #endregion
}
