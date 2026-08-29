using System.Diagnostics.CodeAnalysis;
using Initialization.Microsoft.Extensions.DependencyInjection.Extensions;
using Mediator;
using MEL.Spectre;
using MEL.Spectre.Theme;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Http;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Options.Validators;
using ModularPipelines.PipelineCli;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using ModularPipelines.Validation;
using Spectre.Console;

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
        RegisterValidationServices(services);
        RegisterDistributedServices(services);
    }

    internal static void ConfigureSpectreConsoleLogger(SpectreConsoleLoggerOptions config) =>
        ConfigureSpectreConsoleLogger(config, DelegatingAnsiConsole.Instance);

    internal static void ConfigureSpectreConsoleLogger(
        SpectreConsoleLoggerOptions config,
        IAnsiConsole console)
    {
        // Console width and CI capabilities are configured by ConsoleCoordinator.
        config.Console = console;
        config.Template = "[{Level:u4}] {Message}";
        config.ExceptionFormats = ExceptionFormats.Default;
        config.AllowMarkupInMessageTemplate = true;
        config.IncludeScopes = false;
        config.IncludeActivity = false;
        config.Theme = new SpectreTheme()
            .WithPlaceholders(placeholders =>
            {
                placeholders.ForName("CommandOutput", Style.Plain);
                placeholders.ForName("CommandError", Style.Plain);
            });
        config.WriteMode = WriteMode.Synchronous;
    }

    /// <summary>
    /// Registers external integrations and bundled services:
    /// options configuration, logging provider, HTTP clients, initializers, and mediator.
    /// </summary>
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "MEL.Spectre options are configured programmatically with statically referenced types.")]
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "MEL.Spectre options are configured programmatically without configuration binding.")]
    private static void RegisterBundledServices(IServiceCollection services)
    {
        services.TryAddSingleton(PipelineCommandLineOptions.Empty);

        services
            .AddSingleton<PipelineCommandHandler>()
            .AddSingleton<PipelinePlanPrinter>()
            .Configure<PipelineOptions>(_ => { })
            .Configure<SchedulerOptions>(_ => { })
            .Configure<ConcurrencyOptions>(_ => { })
            .Configure<HttpResilienceOptions>(_ => { })
            .AddSingleton<IValidateOptions<ConcurrencyOptions>, ConcurrencyOptionsValidator>()
            .AddSingleton<IValidateOptions<HttpResilienceOptions>, HttpResilienceOptionsValidator>()
            .AddLogging(builder =>
            {
                builder.ClearProviders();

                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);

                builder.AddSpectreConsole(ConfigureSpectreConsoleLogger);
            })
            .AddHttpClient()
            .AddLoggingHttpClients()
            .AddInitializers()
            .AddServiceCollection()
            .AddMediator();
    }

    /// <summary>
    /// Registers scoped services injected into IModuleContext:
    /// HTTP, command execution, installers, shell environments, and context-specific services.
    /// </summary>
    private static void RegisterPipelineContextServices(IServiceCollection services)
    {
        services.TryAddSingleton(new PipelineWorkingDirectory(Directory.GetCurrentDirectory()));

        services
            .AddSingleton<ModuleLookup>()
            .AddScoped<PipelineContext>()
            .AddScoped<IPipelineContext>(sp => sp.GetRequiredService<PipelineContext>())
            .AddScoped<ModuleLoggerAccessor>()
            .AddScoped<IModuleLoggerAccessor>(sp => sp.GetRequiredService<ModuleLoggerAccessor>())
            .AddScoped<IInternalModuleLoggerAccessor>(sp => sp.GetRequiredService<ModuleLoggerAccessor>())
            .AddScoped(typeof(ModuleLogger<>))
            .AddScoped<IHttpContext, Http.Http>()
            .AddScoped<ICommandContext, Command>()
            .AddScoped<ICommandLineBuilder, CommandLineBuilder>()
            .AddScoped<ICommandLogger, CommandLogger>()
            .AddScoped<ICertificatesContext, Certificates>()
            .AddScoped<IDownloaderContext, Downloader>()
            .AddScoped<IZipContext, Zip>()
            .AddScoped<IPowerShellContext, PowerShell>()
            .AddScoped<IBashContext, Bash>()
            .AddScoped<ModularPipelines.Context.IShellContext, ModularPipelines.Context.Domains.Implementations.ShellContext>()
            .AddScoped<IFilesContext, FilesContext>()
            .AddSingleton<IDataContext, DataContext>()
            .AddSingleton<IBuildSystemContext, BuildSystemContext>()
            .AddScoped<IEnvironmentContext, EnvironmentContext>()
            .AddScoped<IInstallersContext, InstallersContext>()
            .AddScoped<INetworkContext, NetworkContext>()
            .AddScoped<ISecurityContext, SecurityContext>()

            // Register Services domain context
            .AddScoped<IServicesContext, ServicesContext>()
            .AddScoped<ModularPipelines.Http.IHttpLogger, ModularPipelines.Http.HttpLogger>()
            .AddScoped<ModularPipelines.Http.IHttpRequestFormatter, ModularPipelines.Http.HttpRequestFormatter>()
            .AddScoped<ModularPipelines.Http.IHttpResponseFormatter, ModularPipelines.Http.HttpResponseFormatter>();
    }

    /// <summary>
    /// Registers logging, console output, and exception formatting services:
    /// console printers, progress reporting, log containers, and exception handling.
    /// </summary>
    private static void RegisterLoggingAndConsoleServices(IServiceCollection services)
    {
        services.AddOptions<SpectreConsoleLoggerOptions>()
            .Configure<IAnsiConsole>((options, console) => options.Console = console);

        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IExceptionOutputFormatter, SpectreExceptionFormatter>()
            .AddSingleton<IStackTraceModuleDetector, StackTraceModuleDetector>()
            .AddSingleton<IConsolePrinter, ConsolePrinter>()
            .AddSingleton<SummaryLogger>()
            .AddSingleton<ISummaryLogger>(sp => sp.GetRequiredService<SummaryLogger>())
            .AddSingleton<ISummaryLogReader>(sp => sp.GetRequiredService<SummaryLogger>())
            .AddSingleton<IInternalSummaryLogger>(sp => sp.GetRequiredService<SummaryLogger>())
            .AddSingleton<IExceptionBuffer, ExceptionBuffer>()
            .AddSingleton<IPrimaryExceptionContainer, PrimaryExceptionContainer>()
            .AddSingleton<ISecondaryExceptionContainer, SecondaryExceptionContainer>()
            .AddSingleton<IExceptionRethrowService, ExceptionRethrowService>()
            .AddSingleton<IAnsiConsole>(DelegatingAnsiConsole.Instance)

            // Console coordinator - single point of control for all console output
            .AddSingleton<Console.ConsoleCoordinator>()
            .AddSingleton<Console.IConsoleCoordinator>(sp => sp.GetRequiredService<Console.ConsoleCoordinator>())
            .AddSingleton<Console.IModuleOutputExcerptProvider>(sp => sp.GetRequiredService<Console.ConsoleCoordinator>())
            .AddSingleton<IProgressDisplay>(sp => sp.GetRequiredService<Console.ConsoleCoordinator>())

            // Output coordinator - manages immediate output during live display
            .AddSingleton<Console.INonSpectreLoggerFactory, Console.NonSpectreLoggerFactory>()
            .AddSingleton<Console.OutputCoordinator>()
            .AddSingleton<Console.IOutputCoordinator>(sp => sp.GetRequiredService<Console.OutputCoordinator>())

            // Progress display components
            .AddSingleton<IProgressCalculator, ProgressCalculator>()
            .AddSingleton<IResultsPrinter, SpectreResultsPrinter>()
            .AddSingleton<ProgressPrinter>()
            .AddSingleton<IProgressPrinter>(sp => sp.GetRequiredService<ProgressPrinter>())
            .AddSingleton<ILogoPrinter, LogoPrinter>()
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
            .AddSingleton<PipelineExecutionState>()
            .AddSingleton<ModuleDiscoveryPlanner>()
            .AddSingleton<DependencyGraphExporter>()
            .AddSingleton<IDependencyGraphExporter>(provider =>
                provider.GetRequiredService<DependencyGraphExporter>())
            .AddSingleton<IDependencyTreeFormatter, DependencyTreeFormatter>();
    }

    /// <summary>
    /// Registers module lifecycle and pipeline execution services:
    /// module retrieval, initialization, execution, scheduling, disposal, and result handling.
    /// </summary>
    private static void RegisterModuleExecutionServices(IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystemProvider>(SystemFileSystemProvider.Instance);
        services

            // Module activator - sets AsyncLocal context before module construction
            .AddSingleton<IModuleActivator, ModuleActivator>()
            .AddSingleton<IPipelineContextProvider, ModuleContextProvider>()
            .AddSingleton<IRequirementChecker, RequirementChecker>()
            .AddSingleton<ModuleRetriever>()
            .AddSingleton<PipelinePlanner>()
            .AddSingleton<ModulePlanningSkipEvaluator>()
            .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddSingleton<IPipelineInitializer, PipelineInitializer>()
            .AddSingleton<IExecutionOrchestrator, ExecutionOrchestrator>()
            .AddSingleton<IPrintProgressExecutor, PrintProgressExecutor>()
            .AddSingleton<IModuleDisposeExecutor, ModuleDisposeExecutor>()
            .AddSingleton<IPipelineExecutor, PipelineExecutor>()
            .AddSingleton<IPipelineOutputCoordinator, PipelineOutputCoordinator>()
            .AddSingleton<IIgnoredModuleResultRegistrar, IgnoredModuleResultRegistrar>()
            .AddSingleton<IPipelineSummaryFactory, PipelineSummaryFactory>()
            .AddSingleton<ICommandExecutionCounter, CommandExecutionCounter>()
            .AddSingleton<PipelineRunReportFactory>()
            .AddSingleton<IRunReportService, RunReportService>()
            .AddSingleton<IModuleExecutor, ModuleExecutor>()
            .AddSingleton<IModuleExecutionPipeline, ModuleExecutionPipeline>()
            .AddSingleton<IModuleResultRegistry, ModuleResultRegistry>()
            .AddSingleton<IModuleResultHistoryProvider, ModuleResultHistoryProvider>()
            .AddSingleton<IModuleSchedulerFactory, ModuleSchedulerFactory>()
            .AddSingleton<ModuleDisposer>()
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>()
            .AddSingleton<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>()
            .AddSingleton<IPipelineFileWriter, PipelineFileWriter>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IOptionsProvider, OptionsProvider>()
            .AddSingleton<DistributedConditionRouting>()
            .AddSingleton<IModuleConditionHandler, ModuleConditionHandler>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IEnvironmentVariablesContext, EnvironmentVariables>()
            .AddSingleton<IThreadPoolConfigurator, ThreadPoolConfigurator>()
            .AddSingleton<IParallelLimitProvider, ParallelLimitProvider>()
            .AddSingleton<ICommandModelProvider, CommandModelProvider>()
            .AddSingleton<ICommandArgumentBuilder, CommandArgumentBuilder>()
            .AddSingleton<ICommandPartsProvider, CommandPartsProvider>()
            .AddSingleton<IMetricsCollector, MetricsCollector>()

            // Module execution components (SRP extraction from ModuleExecutor)
            .AddSingleton<Engine.Execution.IModuleResultRegistrar, Engine.Execution.ModuleResultRegistrar>()
            .AddSingleton<Engine.Execution.IParallelLimitHandler, Engine.Execution.ParallelLimitHandler>()
            .AddSingleton<Engine.Execution.IDependencyWaiter, Engine.Execution.DependencyWaiter>()
            .AddSingleton<Engine.Execution.IModuleLifecycleEventInvoker, Engine.Execution.ModuleLifecycleEventInvoker>()
            .AddSingleton<Engine.Execution.IDirectHookInvoker, Engine.Execution.DirectHookInvoker>()
            .AddSingleton<Engine.Execution.IModuleRunner, Engine.Execution.ModuleRunner>()
            .AddSingleton<Engine.Execution.IAlwaysRunHandler, Engine.Execution.AlwaysRunHandler>()

            // Module scheduling components (SRP extraction from ModuleScheduler)
            .AddSingleton<Engine.Scheduling.IModuleConstraintEvaluator, Engine.Scheduling.ModuleConstraintEvaluator>();

        services.TryAddSingleton(static serviceProvider =>
            new RunReportPathResolver(
                serviceProvider.GetRequiredService<PipelineWorkingDirectory>()));
        services.TryAddSingleton<IRunHistoryStore, FileSystemRunHistoryStore>();
        services.TryAddSingleton<IRunHistoryReader, RunHistoryReader>();
    }

    /// <summary>
    /// Registers build system integration services:
    /// build system detection, secret masking, and output formatting.
    /// </summary>
    private static void RegisterBuildSystemServices(IServiceCollection services)
    {
        // Capture before service-provider construction and ConsoleCoordinator.Install() can redirect stdout.
        var commandWriter = new BuildSystemCommandWriter(System.Console.Out);

        services
            .Configure<SecretMaskingOptions>(_ => { })
            .AddSingleton<SecretProvider>()
            .AddSingleton<ISecretProvider>(sp => sp.GetRequiredService<SecretProvider>())
            .AddSingleton<ISecretRegistry>(sp => sp.GetRequiredService<SecretProvider>())
            .AddSingleton<ISecretObfuscator, SecretObfuscator>()
            .AddSingleton<IBuildSystemCommandWriter>(commandWriter)
            .AddSingleton<IBuildSystemSecretMasker, BuildSystemSecretMasker>()
            .AddSingleton<IBuildSystemDetector, BuildSystemDetector>()
            .AddSingleton<IBuildSystemFormatterProvider, BuildSystemFormatterProvider>();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, BuildSystemLogIssueLoggerProvider>());
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

            // IDependencyContext is implemented by IModuleMetadataRegistry - expose it for dependency resolution
            .AddSingleton<IDependencyContext>(sp => sp.GetRequiredService<IModuleMetadataRegistry>())
            .AddSingleton<IModuleAttributeEventService, ModuleAttributeEventService>()
            .AddSingleton<IEventHandlerInvoker, EventHandlerInvoker>()
            .AddSingleton<IRegistrationEventExecutor, RegistrationEventExecutor>();
    }

    /// <summary>
    /// Registers stateless utility services:
    /// encoding (Base64, Hex), hashing, and serialization (JSON, XML, YAML).
    /// </summary>
    private static void RegisterUtilityServices(IServiceCollection services)
    {
        services
            .AddSingleton<IBase64Context, Base64>()
            .AddSingleton<IHexContext, Hex>()
            .AddSingleton<IJsonContext, Json>()
            .AddSingleton<IXmlContext, Xml>()
            .AddSingleton<IYamlContext, Yaml>()
            .AddSingleton<IHashContext, HashContext>()
            .AddSingleton<IToolResolver, ToolResolver>();
    }

    /// <summary>
    /// Registers pipeline validation services:
    /// validators for options, dependencies, and module configuration.
    /// </summary>
    private static void RegisterValidationServices(IServiceCollection services)
    {
        services
            .AddSingleton<IPipelineValidationService, PipelineValidationService>()
            .AddSingleton<IPipelineValidator, OptionsValidator>()
            .AddSingleton<IPipelineValidator, DependencyValidator>()
            .AddSingleton<IPipelineValidator, ArtifactContractValidator>()
            .AddSingleton<IPipelineValidator, ModuleSelectionValidator>()
            .AddSingleton<IPipelineValidator, ModuleConfigurationValidator>();
    }

    /// <summary>
    /// Registers distributed execution infrastructure with local defaults.
    /// These are always available; when distributed mode is not enabled, they are harmless no-ops.
    /// The actual executor replacement happens in <see cref="PipelineBuilder"/> when TotalInstances > 1.
    /// </summary>
    private static void RegisterDistributedServices(IServiceCollection services)
    {
        // Always-on defaults (TryAdd so user/extension can override)
        services.Configure<DistributedOptions>(_ => { });
        services.Configure<ArtifactOptions>(_ => { });
        services.TryAddSingleton<IDistributedCoordinator, InMemoryDistributedCoordinator>();
        services.TryAddSingleton<IDistributedArtifactStore, FileSystemDistributedArtifactStore>();

        // Serialization (always available)
        services.TryAddSingleton<ModuleTypeRegistry>();
        services.TryAddSingleton(sp => new ModuleResultSerializer(
            sp.GetRequiredService<ModuleTypeRegistry>(),
            sp.GetRequiredService<ICommandExecutionCounter>()));

        // Artifact lifecycle manager (handles [ProducesArtifact]/[ConsumesArtifact])
        services.TryAddSingleton<ArtifactLifecycleManager>();

        // Role detection
        services.TryAddSingleton<RoleDetector>();
    }
}
