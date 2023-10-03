using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Options;
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

                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);

                builder.AddSpectreConsole();
            })
            .AddHttpClient()
            .AddInitializers()
            .AddServiceCollection();

        services
            .AddScoped<LoggingHttpHandler>()
            .AddHttpClient("LoggingHttpClient")
            .AddHttpMessageHandler<LoggingHttpHandler>();

        services
            .AddScoped<RequestLoggingHttpHandler>()
            .AddHttpClient("RequestLoggingHttpClient")
            .AddHttpMessageHandler<RequestLoggingHttpHandler>();

        services
            .AddScoped<ResponseLoggingHttpHandler>()
            .AddHttpClient("ResponseLoggingHttpClient")
            .AddHttpMessageHandler<ResponseLoggingHttpHandler>();

        // Transient
        services.AddScoped<IPipelineContext, PipelineContext>()
            .AddScoped<IConsolePrinter, ConsolePrinter>()
            .AddScoped<IModuleLoggerProvider, ModuleLoggerProvider>()
            .AddScoped<IHttp, Http.Http>()
            .AddScoped<ICommand, Command>()
            .AddScoped<ICommandLogger, CommandLogger>()
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
            .AddScoped<IJson, Context.Json>()
            .AddScoped<IXml, Xml>()
            .AddScoped<IYaml, Yaml>()
            .AddScoped<IPowershell, Powershell>()
            .AddScoped<IBash, Bash>()
            .AddScoped<IPipelineContextProvider, ModuleContextProvider>()
            .AddScoped<IChecksum, Checksum>()
            .AddScoped<IEnvironmentContext, EnvironmentContext>()
            .AddScoped<IDependencyChainProvider, DependencyChainProvider>()
            .AddScoped<IDependencyDetector, DependencyDetector>()
            .AddScoped<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddScoped<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddScoped<IDependencyPrinter, DependencyPrinter>()
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
            .AddScoped<IModuleDisposer, ModuleDisposer>()
            .AddScoped<ILogoPrinter, LogoPrinter>()
            .AddScoped<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddScoped<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddScoped<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddScoped<ICollapsableLogging, SmartCollapsableLogging>()
            .AddScoped<IInternalCollapsableLogging, SmartCollapsableLogging>();

        // Singletons
        services
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton(typeof(ModuleLogger<>))
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IOptionsProvider, OptionsProvider>()
            .AddSingleton<ISecretProvider, SecretProvider>()
            .AddSingleton<ISecretObfuscator, SecretObfuscator>()
            .AddSingleton<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<ISmartCollapsableLoggingStringBlockProvider, SmartCollapsableLoggingStringBlockProvider>()
            .AddSingleton<IModuleIgnoreHandler, ModuleIgnoreHandler>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IConsoleWriter, ConsoleWriter>()
            .AddSingleton<IEnvironmentVariables, EnvironmentVariables>();
    }
}