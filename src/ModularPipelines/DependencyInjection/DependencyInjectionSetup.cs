using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;
using Vertical.SpectreLogger;

namespace ModularPipelines.DependencyInjection;

internal static class DependencyInjectionSetup
{
    public static void Initialize(IServiceCollection services)
    {
        // Bundles
        services
            .Configure<PipelineOptions>(_ => { })
            .AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSpectreConsole(cfg =>
                {
                    cfg.WriteInForeground();
                });
            })
            .AddHttpClient()
            .AddInitializers()
            .AddServiceCollection();

        // Transient
        services.AddTransient<IModuleContext, ModuleContext>()
            .AddTransient<IConsolePrinter, ConsolePrinter>()
            .AddTransient<IModuleLoggerProvider, ModuleLoggerProvider>()
            .AddTransient<IHttp, Http>()
            .AddTransient<ICommand, Command>()
            .AddTransient<ICertificates, Certificates>()
            .AddTransient<IDownloader, Downloader>()
            .AddTransient<IInstaller, Installer>()
            .AddTransient<IFileInstaller, FileInstaller>()
            .AddTransient<IPredefinedInstallers, PredefinedInstallers>()
            .AddTransient<IWindowsInstaller, WindowsInstaller>()
            .AddTransient<IMacInstaller, MacInstaller>()
            .AddTransient<ILinuxInstaller, LinuxInstaller>()
            .AddTransient<IAptGet, AptGet>()
            .AddTransient<IHasher, Hasher>()
            .AddTransient<IBase64, Base64>()
            .AddTransient<IHex, Hex>()
            .AddTransient<IZip, Zip>()
            .AddTransient<IJson, Json>()
            .AddTransient<IXml, Xml>()
            .AddTransient<IPowershell, Powershell>()
            .AddTransient<IBash, Bash>()
            .AddTransient<ISecretObfuscator, SecretObfuscator>()
            .AddTransient<IOptionsProvider, OptionsProvider>()
            .AddTransient<IModuleContextProvider, ModuleContextProvider>();

        // Singletons
        services
            .AddSingleton<IModuleDisposer, ModuleDisposer>()
            .AddSingleton<ILogoPrinter, LogoPrinter>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<IModuleStatusProvider, ModuleStatusProvider>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IServiceProviderInitializer, ServiceProviderInitializer>()
            .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddSingleton<IModuleInitializer, ModuleInitializer>()
            .AddSingleton<IModuleIgnoreHandler, ModuleIgnoreHandler>()
            .AddSingleton<IPipelineInitializer, PipelineInitializer>()
            .AddSingleton<IProgressPrinter, ProgressProgressPrinter>()
            .AddSingleton<IExecutionOrchestrator, ExecutionOrchestrator>()
            .AddSingleton<IPrintProgressExecutor, PrintProgressExecutor>()
            .AddSingleton<IPrintModuleOutputExecutor, PrintModuleOutputExecutor>()
            .AddSingleton<IModuleDisposeExecutor, ModuleDisposeExecutor>()
            .AddSingleton<IPipelineExecutor, PipelineExecutor>()
            .AddSingleton<IModuleExecutor, ModuleExecutor>()
            .AddSingleton(typeof(ModuleLogger<>))
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IDependencyChainProvider, DependencyChainProvider>()
            .AddSingleton<IDependencyDetector, DependencyDetector>()
            .AddSingleton<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddSingleton<IDependencyPrinter, DependencyPrinter>()
            .AddSingleton<IEnvironmentContext, EnvironmentContext>()
            .AddSingleton<IEnvironmentVariables, EnvironmentVariables>()
            .AddSingleton<IFileSystemContext, FileSystemContext>()
            .AddSingleton<IRequirementChecker, RequirementChecker>()
            .AddSingleton<IModuleRetriever, ModuleRetriever>()
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddSingleton<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>();
    }
}