using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

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

                builder.AddSpectreConsole(config =>
                {
                    config.ConfigureProfile(LogLevel.Information, profile =>
                    {
                        profile.ConfigureOptions<Vertical.SpectreLogger.Rendering.ExceptionRenderer.Options>(options =>
                        {
                            options.MaxStackFrames = int.MaxValue;
                        });
                    });
                    
                    config.ConfigureProfile(LogLevel.Warning, profile =>
                    {
                        profile.ConfigureOptions<Vertical.SpectreLogger.Rendering.ExceptionRenderer.Options>(options =>
                        {
                            options.MaxStackFrames = int.MaxValue;
                        });
                    });
                    
                    config.ConfigureProfile(LogLevel.Error, profile =>
                    {
                        profile.ConfigureOptions<Vertical.SpectreLogger.Rendering.ExceptionRenderer.Options>(options =>
                        {
                            options.MaxStackFrames = int.MaxValue;
                        });
                    });
                });
            })
            .AddHttpClient()
            .AddInitializers()
            .AddServiceCollection()
            .AddMediator(); // Add Mediator for event handling

        // Everything that is injected into `PipelineContext` should be Scoped
        services
            .AddScoped<IPipelineContext, PipelineContext>()
            .AddScoped<IModuleLoggerProvider, ModuleLoggerProvider>()
            .AddScoped(typeof(ModuleLogger<>))
            .AddScoped<IHttp, Http.Http>()
            .AddScoped<ModularPipelines.Context.ICommand, Command>()
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
            .AddScoped<IJson, Json>()
            .AddScoped<IXml, Xml>()
            .AddScoped<IYaml, Yaml>()
            .AddScoped<IPowershell, Powershell>()
            .AddScoped<IBash, Bash>()
            .AddScoped<IChecksum, Checksum>()
            .AddScoped<IEnvironmentContext, EnvironmentContext>();

        // Singletons
        services
            .AddSingleton<IConsolePrinter, ConsolePrinter>()
            .AddSingleton<IAfterPipelineLogger, AfterPipelineLogger>()
            .AddSingleton<IExceptionBuffer, ExceptionBuffer>()
            .AddSingleton<ISecondaryExceptionContainer, SecondaryExceptionContainer>()
            .AddSingleton<IPipelineContextProvider, ModuleContextProvider>()
            .AddSingleton<IDependencyChainProvider, DependencyChainProvider>()
            .AddSingleton<IDependencyDetector, DependencyDetector>()
            .AddSingleton<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddSingleton<IDependencyPrinter, DependencyPrinter>()
            .AddSingleton<IFileSystemContext, FileSystemContext>()
            .AddSingleton<IRequirementChecker, RequirementChecker>()
            .AddSingleton<IModuleRetriever, ModuleRetriever>()
            .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddSingleton<IModuleInitializer, ModuleInitializer>()
            .AddSingleton<IPipelineInitializer, PipelineInitializer>()
            .AddSingleton<ProgressPrinter>()
            .AddSingleton<IProgressPrinter>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<INotificationHandler<ModuleStartedNotification>>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<INotificationHandler<ModuleCompletedNotification>>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<INotificationHandler<ModuleSkippedNotification>>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<INotificationHandler<SubModuleCreatedNotification>>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<INotificationHandler<SubModuleCompletedNotification>>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<IExecutionOrchestrator, ExecutionOrchestrator>()
            .AddSingleton<IPrintProgressExecutor, PrintProgressExecutor>()
            .AddSingleton<IPrintModuleOutputExecutor, PrintModuleOutputExecutor>()
            .AddSingleton<IModuleDisposeExecutor, ModuleDisposeExecutor>()
            .AddSingleton<IPipelineExecutor, PipelineExecutor>()
            .AddSingleton<IModuleExecutor, ModuleExecutor>()
            .AddSingleton<IModuleDisposer, ModuleDisposer>()
            .AddSingleton<ILogoPrinter, LogoPrinter>()
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddSingleton<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddSingleton<ICollapsableLogging, SmartCollapsableLogging>()
            .AddSingleton<IInternalCollapsableLogging, SmartCollapsableLogging>()
            .AddSingleton<IPipelineFileWriter, PipelineFileWriter>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IOptionsProvider, OptionsProvider>()
            .AddSingleton<ISecretProvider, SecretProvider>()
            .AddSingleton<ISecretObfuscator, SecretObfuscator>()
            .AddSingleton<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<ISmartCollapsableLoggingStringBlockProvider, SmartCollapsableLoggingStringBlockProvider>()
            .AddSingleton<IModuleConditionHandler, ModuleConditionHandler>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IConsoleWriter, ConsoleWriter>()
            .AddSingleton<IEnvironmentVariables, EnvironmentVariables>()
            .AddSingleton<IParallelLimitProvider, ParallelLimitProvider>();
    }
}