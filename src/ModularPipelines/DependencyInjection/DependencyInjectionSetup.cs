using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;
using Vertical.SpectreLogger;

namespace ModularPipelines.DependencyInjection;

internal static class DependencyInjectionSetup
{
    public static void Initialize(IServiceCollection services)
    {
        var secretsLogFilter = new SecretsLogFilter();
        
        // Bundles
        services
            .Configure<PipelineOptions>(_ => { })
            .AddLogging(builder =>
            {
                builder.ClearProviders();

                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
                    
                builder.AddSpectreConsole(cfg =>
                {
                    cfg.SetLogEventFilter(secretsLogFilter);
                });
            })
            .AddHttpClient()
            .AddInitializers()
            .AddServiceCollection();

        services
            .AddScoped<LoggingHttpHandler>()
            .AddHttpClient("LoggingHttpClient")
            .AddHttpMessageHandler<LoggingHttpHandler>();
        
        // Transient
        services.AddScoped<IPipelineContext, PipelineContext>()
            .AddScoped<IConsolePrinter, ConsolePrinter>()
            .AddScoped<IModuleLoggerProvider, ModuleLoggerProvider>()
            .AddScoped<IHttp, Http.Http>()
            .AddScoped<ICommand, Command>()
            .AddScoped<ICertificates, Certificates>()
            .AddScoped<IDownloader, Downloader>()
            .AddScoped<IInstaller, Installer>()
            .AddScoped<IFileInstaller, FileInstaller>()
            .AddScoped<IPredefinedInstallers, PredefinedInstallers>()
            .AddScoped<IWindowsInstaller, WindowsInstaller>()
            .AddScoped<IMacInstaller, MacInstaller>()
            .AddScoped<ILinuxInstaller, LinuxInstaller>()
            .AddScoped<IAptGet, AptGet>()
            .AddScoped<IHasher, Hasher>()
            .AddScoped<IBase64, Base64>()
            .AddScoped<IHex, Hex>()
            .AddScoped<IZip, Zip>()
            .AddScoped<IJson, Json>()
            .AddScoped<IXml, Xml>()
            .AddScoped<IYaml, Yaml>()
            .AddScoped<IPowershell, Powershell>()
            .AddScoped<IBash, Bash>()
            .AddScoped<ISecretObfuscator, SecretObfuscator>()
            .AddScoped<IPipelineContextProvider, ModuleContextProvider>()
            .AddScoped<IChecksum, Checksum>()
            .AddScoped<IEnvironmentContext, EnvironmentContext>()
            .AddScoped<IDependencyChainProvider, DependencyChainProvider>()
            .AddScoped<IDependencyDetector, DependencyDetector>()
            .AddScoped<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddScoped<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddScoped<IDependencyPrinter, DependencyPrinter>()
            .AddScoped<IEnvironmentVariables, EnvironmentVariables>()
            .AddScoped<IFileSystemContext, FileSystemContext>()
            .AddScoped<IRequirementChecker, RequirementChecker>()
            .AddScoped<IModuleRetriever, ModuleRetriever>()
            .AddScoped<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddScoped<IModuleInitializer, ModuleInitializer>()
            .AddScoped<IPipelineInitializer, PipelineInitializer>()
            .AddScoped<IProgressPrinter, ProgressPrinter>()
            .AddScoped<IExecutionOrchestrator, ExecutionOrchestrator>()
            .AddScoped<IPrintProgressExecutor, PrintProgressExecutor>()
            .AddScoped<IPrintModuleOutputExecutor, PrintModuleOutputExecutor>()
            .AddScoped<IModuleDisposeExecutor, ModuleDisposeExecutor>()
            .AddScoped<IPipelineExecutor, PipelineExecutor>()
            .AddScoped<IModuleExecutor, ModuleExecutor>()
            .AddScoped<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddScoped<IModuleDisposer, ModuleDisposer>()
            .AddScoped<ILogoPrinter, LogoPrinter>()
            .AddScoped<IBuildSystemDetector, BuildSystemDetector>()
            .AddScoped<IModuleStatusProvider, ModuleStatusProvider>()
            .AddScoped<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddScoped<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddScoped<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddScoped<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddScoped<IModuleIgnoreHandler, ModuleIgnoreHandler>();

        // Singletons
        services
            .AddSingleton(secretsLogFilter)
            .AddScoped<EngineCancellationToken>()
            .AddScoped(typeof(ModuleLogger<>))
            .AddScoped<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddScoped<IServiceProviderInitializer, ServiceProviderInitializer>()
            .AddScoped<IOptionsProvider, OptionsProvider>()
            .AddScoped<ISecretProvider, SecretProvider>();
    }
}