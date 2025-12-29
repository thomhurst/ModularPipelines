using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Helpers.Internal;
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
            .Configure<SchedulerOptions>(_ => { })
            .AddLogging(builder =>
            {
                builder.ClearProviders();

                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);

                builder.AddSpectreConsole(config =>
                {
                    config.ConfigureProfiles(profile =>
                    {
                        // Enable Spectre.Console markup rendering in log messages
                        profile.PreserveMarkupInFormatStrings = true;

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
            .AddScoped<IZip, Zip>()
            .AddScoped<IPowershell, Powershell>()
            .AddScoped<IBash, Bash>()
            .AddScoped<IEnvironmentContext, EnvironmentContext>()
            .AddScoped<ModularPipelines.Http.IHttpLogger, ModularPipelines.Http.HttpLogger>()
            .AddScoped<ModularPipelines.Http.IHttpRequestFormatter, ModularPipelines.Http.HttpRequestFormatter>()
            .AddScoped<ModularPipelines.Http.IHttpResponseFormatter, ModularPipelines.Http.HttpResponseFormatter>()
            .AddScoped<ILogEventBuffer, LogEventBuffer>();

        // Singletons
        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IExceptionOutputFormatter, SpectreExceptionFormatter>()
            .AddSingleton<IStackTraceModuleDetector, StackTraceModuleDetector>()
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

            // INotificationHandler registrations removed - Mediator auto-discovers them via source generators
            .AddSingleton<IExecutionOrchestrator, ExecutionOrchestrator>()
            .AddSingleton<IPrintProgressExecutor, PrintProgressExecutor>()
            .AddSingleton<IPrintModuleOutputExecutor, PrintModuleOutputExecutor>()
            .AddSingleton<IModuleDisposeExecutor, ModuleDisposeExecutor>()
            .AddSingleton<IPipelineExecutor, PipelineExecutor>()
            .AddSingleton<IPipelineOutputCoordinator, PipelineOutputCoordinator>()
            .AddSingleton<IIgnoredModuleResultRegistrar, IgnoredModuleResultRegistrar>()
            .AddSingleton<IPipelineSummaryFactory, PipelineSummaryFactory>()
            .AddSingleton<IModuleExecutor, ModuleExecutor>()
            .AddSingleton<IModuleExecutionPipeline, ModuleExecutionPipeline>()
            .AddSingleton<IModuleResultRegistry, ModuleResultRegistry>()
            .AddSingleton<IModuleSchedulerFactory, ModuleSchedulerFactory>()
            .AddSingleton<IModuleDisposer, ModuleDisposer>()
            .AddSingleton<ILogoPrinter, LogoPrinter>()
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddSingleton<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddSingleton<IPipelineFileWriter, PipelineFileWriter>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IModuleOutputWriterFactory, ModuleOutputWriterFactory>()
            .AddSingleton<IOptionsProvider, OptionsProvider>()
            .AddSingleton<ISecretProvider, SecretProvider>()
            .AddSingleton<ISecretObfuscator, SecretObfuscator>()
            .AddSingleton<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<IBuildSystemFormatterProvider, BuildSystemFormatterProvider>()
            .AddSingleton<IModuleConditionHandler, ModuleConditionHandler>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IConsoleWriter, ConsoleWriter>()
            .AddSingleton<IEnvironmentVariables, EnvironmentVariables>()
            .AddSingleton<IParallelLimitProvider, ParallelLimitProvider>()
            .AddSingleton<IFormattedLogValuesObfuscator, FormattedLogValuesObfuscator>()
            .AddSingleton<IDependencyTreeFormatter, DependencyTreeFormatter>()
            .AddSingleton<ICommandModelProvider, CommandModelProvider>()
            .AddSingleton<ICommandArgumentBuilder, CommandArgumentBuilder>()

            // Attribute event system
            .AddSingleton<IModuleDependencyRegistry, ModuleDependencyRegistry>()
            .AddSingleton<IModuleMetadataRegistry, ModuleMetadataRegistry>()
            .AddSingleton<IModuleAttributeEventService, ModuleAttributeEventService>()
            .AddSingleton<IAttributeEventInvoker, AttributeEventInvoker>()
            .AddSingleton<IRegistrationEventExecutor, RegistrationEventExecutor>()

            // Module execution components (SRP extraction from ModuleExecutor)
            .AddSingleton<Engine.Execution.IModuleResultRegistrar, Engine.Execution.ModuleResultRegistrar>()
            .AddSingleton<Engine.Execution.IParallelLimitHandler, Engine.Execution.ParallelLimitHandler>()
            .AddSingleton<Engine.Execution.IDependencyWaiter, Engine.Execution.DependencyWaiter>()
            .AddSingleton<Engine.Execution.IModuleLifecycleEventInvoker, Engine.Execution.ModuleLifecycleEventInvoker>()
            .AddSingleton<Engine.Execution.IModuleRunner, Engine.Execution.ModuleRunner>()
            .AddSingleton<Engine.Execution.IAlwaysRunHandler, Engine.Execution.AlwaysRunHandler>()

            // Module scheduling components (SRP extraction from ModuleScheduler)
            .AddSingleton<Engine.Scheduling.IModuleConstraintEvaluator, Engine.Scheduling.ModuleConstraintEvaluator>()

            // Metrics collection
            .AddSingleton<IMetricsCollector, MetricsCollector>()

            // Stateless utilities - safe as singletons
            .AddSingleton<IBase64, Base64>()
            .AddSingleton<IHex, Hex>()
            .AddSingleton<IChecksum, Checksum>()
            .AddSingleton<IJson, Json>()
            .AddSingleton<IXml, Xml>()
            .AddSingleton<IYaml, Yaml>()
            .AddSingleton<IHasher, Hasher>();
    }
}