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
    /// <summary>
    /// Initializes all dependency injection registrations for the ModularPipelines framework.
    /// Registrations are organized into logical groups for maintainability.
    /// </summary>
    public static void Initialize(IServiceCollection services)
    {
        RegisterBundledServices(services);
        RegisterPipelineContextServices(services);
        RegisterLoggingAndConsoleServices(services);
        RegisterDependencyManagementServices(services);
        RegisterModuleExecutionServices(services);
        RegisterBuildSystemServices(services);
        RegisterAttributeEventServices(services);
        RegisterUtilityServices(services);
    }

    /// <summary>
    /// Registers external integrations and bundled services:
    /// options configuration, logging provider, HTTP clients, initializers, and mediator.
    /// </summary>
    private static void RegisterBundledServices(IServiceCollection services)
    {
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
            .AddLoggingHttpClients()
            .AddInitializers()
            .AddServiceCollection()
            .AddMediator();
    }

    /// <summary>
    /// Registers scoped services injected into IPipelineContext:
    /// HTTP, command execution, installers, shell environments, and context-specific services.
    /// </summary>
    private static void RegisterPipelineContextServices(IServiceCollection services)
    {
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
    }

    /// <summary>
    /// Registers logging, console output, and exception formatting services:
    /// console printers, progress reporting, log containers, and exception handling.
    /// </summary>
    private static void RegisterLoggingAndConsoleServices(IServiceCollection services)
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IExceptionOutputFormatter, SpectreExceptionFormatter>()
            .AddSingleton<IStackTraceModuleDetector, StackTraceModuleDetector>()
            .AddSingleton<IConsolePrinter, ConsolePrinter>()
            .AddSingleton<IAfterPipelineLogger, AfterPipelineLogger>()
            .AddSingleton<IExceptionBuffer, ExceptionBuffer>()
            .AddSingleton<IPrimaryExceptionContainer, PrimaryExceptionContainer>()
            .AddSingleton<ISecondaryExceptionContainer, SecondaryExceptionContainer>()
            .AddSingleton<ProgressPrinter>()
            .AddSingleton<IProgressPrinter>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<ILogoPrinter, LogoPrinter>()
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IOutputFlushLock, OutputFlushLock>()
            .AddSingleton<IModuleOutputWriterFactory, ModuleOutputWriterFactory>()
            .AddSingleton<IConsoleWriter, ConsoleWriter>()
            .AddSingleton<IFormattedLogValuesObfuscator, FormattedLogValuesObfuscator>();
    }

    /// <summary>
    /// Registers dependency graph management services:
    /// dependency chain resolution, collision detection, unused module detection, and tree formatting.
    /// </summary>
    private static void RegisterDependencyManagementServices(IServiceCollection services)
    {
        services
            .AddSingleton<IDependencyChainProvider, DependencyChainProvider>()
            .AddSingleton<IDependencyDetector, DependencyDetector>()
            .AddSingleton<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddSingleton<IDependencyPrinter, DependencyPrinter>()
            .AddSingleton<IDependencyTreeFormatter, DependencyTreeFormatter>();
    }

    /// <summary>
    /// Registers module lifecycle and pipeline execution services:
    /// module retrieval, initialization, execution, scheduling, disposal, and result handling.
    /// </summary>
    private static void RegisterModuleExecutionServices(IServiceCollection services)
    {
        services
            .AddSingleton<IPipelineContextProvider, ModuleContextProvider>()
            .AddSingleton<IFileSystemContext, FileSystemContext>()
            .AddSingleton<IRequirementChecker, RequirementChecker>()
            .AddSingleton<IModuleRetriever, ModuleRetriever>()
            .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddSingleton<IModuleInitializer, ModuleInitializer>()
            .AddSingleton<IPipelineInitializer, PipelineInitializer>()
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
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddSingleton<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddSingleton<IPipelineFileWriter, PipelineFileWriter>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IOptionsProvider, OptionsProvider>()
            .AddSingleton<IModuleConditionHandler, ModuleConditionHandler>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IEnvironmentVariables, EnvironmentVariables>()
            .AddSingleton<IParallelLimitProvider, ParallelLimitProvider>()
            .AddSingleton<ICommandModelProvider, CommandModelProvider>()
            .AddSingleton<ICommandArgumentBuilder, CommandArgumentBuilder>()
            .AddSingleton<IMetricsCollector, MetricsCollector>()

            // Module execution components (SRP extraction from ModuleExecutor)
            .AddSingleton<Engine.Execution.IModuleResultRegistrar, Engine.Execution.ModuleResultRegistrar>()
            .AddSingleton<Engine.Execution.IParallelLimitHandler, Engine.Execution.ParallelLimitHandler>()
            .AddSingleton<Engine.Execution.IDependencyWaiter, Engine.Execution.DependencyWaiter>()
            .AddSingleton<Engine.Execution.IModuleLifecycleEventInvoker, Engine.Execution.ModuleLifecycleEventInvoker>()
            .AddSingleton<Engine.Execution.IModuleRunner, Engine.Execution.ModuleRunner>()
            .AddSingleton<Engine.Execution.IAlwaysRunHandler, Engine.Execution.AlwaysRunHandler>()

            // Module scheduling components (SRP extraction from ModuleScheduler)
            .AddSingleton<Engine.Scheduling.IModuleConstraintEvaluator, Engine.Scheduling.ModuleConstraintEvaluator>();
    }

    /// <summary>
    /// Registers build system integration services:
    /// build system detection, secret masking, and output formatting.
    /// </summary>
    private static void RegisterBuildSystemServices(IServiceCollection services)
    {
        services
            .AddSingleton<ISecretProvider, SecretProvider>()
            .AddSingleton<ISecretObfuscator, SecretObfuscator>()
            .AddSingleton<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<IBuildSystemFormatterProvider, BuildSystemFormatterProvider>();
    }

    /// <summary>
    /// Registers attribute-based event system services:
    /// module metadata registry, dependency registry, and attribute event handling.
    /// </summary>
    private static void RegisterAttributeEventServices(IServiceCollection services)
    {
        services
            .AddSingleton<IModuleDependencyRegistry, ModuleDependencyRegistry>()
            .AddSingleton<IModuleMetadataRegistry, ModuleMetadataRegistry>()
            .AddSingleton<IModuleAttributeEventService, ModuleAttributeEventService>()
            .AddSingleton<IAttributeEventInvoker, AttributeEventInvoker>()
            .AddSingleton<IRegistrationEventExecutor, RegistrationEventExecutor>();
    }

    /// <summary>
    /// Registers stateless utility services:
    /// encoding (Base64, Hex), hashing (Checksum, Hasher), and serialization (JSON, XML, YAML).
    /// </summary>
    private static void RegisterUtilityServices(IServiceCollection services)
    {
        services
            .AddSingleton<IBase64, Base64>()
            .AddSingleton<IHex, Hex>()
            .AddSingleton<IChecksum, Checksum>()
            .AddSingleton<IJson, Json>()
            .AddSingleton<IXml, Xml>()
            .AddSingleton<IYaml, Yaml>()
            .AddSingleton<IHasher, Hasher>();
    }
}