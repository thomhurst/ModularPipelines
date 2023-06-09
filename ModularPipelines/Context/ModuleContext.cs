using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace ModularPipelines.Context;

internal class ModuleContext : IModuleContext
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private ILogger? _logger;

    public ILogger Logger => _logger ?? _moduleLoggerProvider.GetLogger();

    public IServiceProvider ServiceProvider { get; }

    public IConfiguration Configuration { get; }

    public IOptions<PipelineOptions> PipelineOptions { get; }

    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IEnvironmentContext Environment { get; }

    public IHasher Hasher { get; }
    public IJson Json { get; }
    public IXml Xml { get; }
    public IModuleResultRepository ModuleResultRepository { get; }
    public ICommand Command { get; }
    public IInstaller Installer { get; }
    public IZip Zip { get; }
    public IHex Hex { get; }
    public IBase64 Base64 { get; }

    public void FetchLogger(Type getType)
    {
        _logger = _moduleLoggerProvider.GetLogger(getType);
    }

    public T Get<T>()
    {
        return (T) ServiceProvider.GetRequiredService(typeof(T));
    }

    public IFileSystemContext FileSystem { get; }

    public ModuleContext(IServiceProvider serviceProvider,
        IDependencyCollisionDetector dependencyCollisionDetector,
        IEnvironmentContext environment,
        IFileSystemContext fileSystem,
        IConfiguration configuration,
        IOptions<PipelineOptions> pipelineOptions,
        IModuleResultRepository moduleResultRepository,
        ICommand command,
        IModuleLoggerProvider moduleLoggerProvider,
        IZip zip,
        IHex hex,
        IBase64 base64,
        IHasher hasher, IJson json, IXml xml, EngineCancellationToken engineCancellationToken, IInstaller installer)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        Zip = zip;
        Hex = hex;
        Base64 = base64;
        Hasher = hasher;
        Json = json;
        Xml = xml;
        EngineCancellationToken = engineCancellationToken;
        Installer = installer;
        ModuleResultRepository = moduleResultRepository;
        Command = command;
        Configuration = configuration;
        PipelineOptions = pipelineOptions;
        ServiceProvider = serviceProvider;
        DependencyCollisionDetector = dependencyCollisionDetector;
        Environment = environment;
        FileSystem = fileSystem;
    }

    public EngineCancellationToken EngineCancellationToken { get; }

    public TModule? GetModule<TModule>() where TModule : ModuleBase
    {
        return ServiceProvider.GetServices<ModuleBase>().OfType<TModule>().SingleOrDefault();
    }

    public ModuleBase? GetModule(Type type)
    {
        return ServiceProvider.GetServices<ModuleBase>().SingleOrDefault(module => module.GetType() == type);
    }
}
