using ModularPipelines.Context;
using Mediator;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

using ModularPipelines.Generated;

namespace ModularPipelines.Testing;

/// <summary>
/// Creates isolated module test builders.
/// </summary>
public static class ModuleTester
{
    /// <summary>
    /// Creates a type-erased test builder.
    /// </summary>
    /// <typeparam name="TModule">The module to execute.</typeparam>
    /// <returns>A module test builder.</returns>
    public static ModuleTestBuilder<TModule> For<TModule>()
        where TModule : class, IModule =>
        new();

    /// <summary>
    /// Creates a strongly typed test builder.
    /// </summary>
    /// <typeparam name="TModule">The module to execute.</typeparam>
    /// <typeparam name="TResult">The module value type.</typeparam>
    /// <returns>A strongly typed module test builder.</returns>
    public static ModuleTestBuilder<TModule, TResult> For<TModule, TResult>()
        where TModule : Module<TResult> =>
        new();
}

/// <summary>
/// Configures isolated execution of one module.
/// </summary>
/// <typeparam name="TModule">The module to execute.</typeparam>
public class ModuleTestBuilder<TModule>
    where TModule : class, IModule
{
    private readonly List<Action<PipelineBuilder>> _registrations = [];
    private readonly List<IDependencySeed> _dependencySeeds = [];
    private readonly Dictionary<(Type ProducerModule, string ArtifactName), byte[]> _artifactSeeds = [];
    private Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>>? _commandHandler;

    /// <summary>
    /// Registers a service instance for the module.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="service">The service instance.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithService<TService>(TService service)
        where TService : class
    {
        _registrations.Add(builder => builder.Services.AddSingleton(service));
        return this;
    }

    /// <summary>
    /// Seeds a successful dependency result without executing the dependency.
    /// </summary>
    /// <typeparam name="TDependency">The dependency module.</typeparam>
    /// <typeparam name="TResult">The dependency value type.</typeparam>
    /// <param name="value">The dependency value.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithDependencyResult<TDependency, TResult>(TResult value)
        where TDependency : Module<TResult>
    {
        _registrations.Add(static builder => builder.AddModule<TDependency>());
        _dependencySeeds.Add(new DependencySeed<TDependency, TResult>(value));
        return this;
    }

    /// <summary>
    /// Seeds a single-file artifact consumed by the module.
    /// </summary>
    /// <typeparam name="TProducer">The module that declares the produced artifact.</typeparam>
    /// <param name="artifactName">The artifact name from <see cref="ConsumesArtifactAttribute"/>.</param>
    /// <param name="contents">The UTF-8 artifact contents.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithArtifact<TProducer>(
        string artifactName,
        string contents)
        where TProducer : class, IModule
    {
        ArgumentNullException.ThrowIfNull(contents);
        return WithArtifact<TProducer>(artifactName, Encoding.UTF8.GetBytes(contents));
    }

    /// <summary>
    /// Seeds a single-file artifact consumed by the module.
    /// </summary>
    /// <typeparam name="TProducer">The module that declares the produced artifact.</typeparam>
    /// <param name="artifactName">The artifact name from <see cref="ConsumesArtifactAttribute"/>.</param>
    /// <param name="contents">The binary artifact contents.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithArtifact<TProducer>(
        string artifactName,
        byte[] contents)
        where TProducer : class, IModule
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactName);
        ArgumentNullException.ThrowIfNull(contents);
        _artifactSeeds[(typeof(TProducer), artifactName)] = contents.ToArray();
        return this;
    }

    /// <summary>
    /// Intercepts every command and returns a caller-provided result.
    /// </summary>
    /// <param name="handler">The command handler.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> InterceptCommands(Func<CommandInvocation, CommandResult> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _commandHandler = (invocation, _) => ValueTask.FromResult(handler(invocation));
        return this;
    }

    /// <summary>
    /// Intercepts every command and returns a caller-provided asynchronous result.
    /// </summary>
    /// <param name="handler">The asynchronous command handler.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> InterceptCommands(
        Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _commandHandler = handler;
        return this;
    }

    /// <summary>
    /// Executes the module in an isolated test pipeline.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The captured module run.</returns>
    public async Task<ModuleTestRun> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var execution = await ExecuteCoreAsync(cancellationToken).ConfigureAwait(false);
        return new ModuleTestRun(execution.Result, execution.Commands, execution.FileSystem);
    }

    internal async Task<ExecutionOutcome> ExecuteCoreAsync(CancellationToken cancellationToken)
    {
        var fileSystem = new InMemoryFileSystemProvider();
        var recorder = new RecordingCommandInterceptor();
        if (_commandHandler is not null)
        {
            recorder.SetHandler(_commandHandler);
        }

        var builder = Pipeline.CreateBuilderWithoutProjectInference(new PipelineBuilderSettings
        {
            EnableCommandLineOptions = false,
        });

        builder.ConfigureOptions(options => options with
        {
            Console = options.Console with
            {
                ShowProgress = false,
                PrintResults = false,
                PrintLogo = false,
                PrintDependencyChains = false,
            },
            ThrowOnPipelineFailure = false,
        });
        builder.Logging.ClearProviders();
        builder.Services.Replace(ServiceDescriptor.Singleton<IFileSystemProvider>(fileSystem));
        builder.Services.AddSingleton(recorder);
        builder.Services.AddSingleton<ICommandInterceptor>(recorder);
        builder.Services.AddSingleton<IConsoleCoordinator>(NoOpConsoleServices.Instance);
        builder.Services.AddSingleton<IModuleOutputExcerptProvider>(NoOpConsoleServices.Instance);
        builder.Services.AddSingleton<IOutputCoordinator>(NoOpConsoleServices.Instance);
        builder.Services.AddSingleton<IProgressDisplay>(NoOpConsoleServices.Instance);
        builder.AddModule<TModule>();

        foreach (var registration in _registrations)
        {
            registration(builder);
        }

        await using var pipeline = await builder.BuildAsync().ConfigureAwait(false);

        foreach (var dependencySeed in _dependencySeeds)
        {
            dependencySeed.Apply(pipeline.Services);
        }

        var module = pipeline.Services.GetServices<IModule>()
            .OfType<TModule>()
            .Single();
        ValidateRequiredDependencyResults(module, pipeline.Services);
        var executionContext = ExecutionContextFactory.Create(module, typeof(TModule));
        if (cancellationToken.CanBeCanceled)
        {
            var originalCancellationTokenSource =
                executionContext.ModuleCancellationTokenSource;
            executionContext.ModuleCancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            originalCancellationTokenSource.Dispose();
        }

        var pipelineContext = pipeline.Services.GetRequiredService<IPipelineContext>();
        var logger = GetModuleLogger(pipeline.Services);
        var mediator = pipeline.Services.GetRequiredService<IMediator>();
        var estimatedTimeProvider = pipeline.Services.GetRequiredService<ISafeModuleEstimatedTimeProvider>();
        var moduleContext = new ModuleContext(
            pipelineContext,
            module,
            executionContext,
            logger,
            mediator,
            estimatedTimeProvider);
        var executionPipeline = pipeline.Services.GetRequiredService<IModuleExecutionPipeline>();
        var executor = ModuleExecutionDelegateFactory.GetExecutor(module.ResultType);
        var effectiveFileSystem = pipeline.Services.GetRequiredService<IFileSystemProvider>();
        var workingDirectory = pipeline.Services
            .GetRequiredService<IOptions<ModuleCacheOptions>>()
            .Value.WorkingDirectory;

        using var outputScope = new ModuleOutputContextScope(typeof(TModule), logger);

        IModuleResult result;
        try
        {
            result = await executor(
                    executionPipeline,
                    module,
                    executionContext,
                    moduleContext,
                    token => RestoreConsumedArtifactsAsync(
                        module,
                        effectiveFileSystem,
                        workingDirectory,
                        token),
                    finalizeExecutionAsync: null,
                    completeModule: true,
                    cancellationToken: CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch when (executionContext.ExecutionTask.IsCompletedSuccessfully)
        {
            result = await executionContext.ExecutionTask.ConfigureAwait(false);
        }

        pipeline.Services.GetRequiredService<IModuleResultRegistry>()
            .RegisterResult(typeof(TModule), result);

        return new ExecutionOutcome(result, recorder.Commands, effectiveFileSystem);
    }

    private async Task RestoreConsumedArtifactsAsync(
        IModule module,
        IFileSystemProvider fileSystem,
        string workingDirectory,
        CancellationToken cancellationToken)
    {
        var attributes = module.GetType()
            .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
            .Cast<ConsumesArtifactAttribute>();
        foreach (var attribute in attributes)
        {
            if (!_artifactSeeds.TryGetValue(
                    (attribute.ProducerModule, attribute.ArtifactName),
                    out var contents))
            {
                throw new InvalidOperationException(
                    $"Artifact '{attribute.ArtifactName}' from module "
                    + $"'{attribute.ProducerModule.FullName}' was not seeded for consumer "
                    + $"'{module.GetType().Name}'. Call WithArtifact to seed it.");
            }

            var restorePath = ResolveArtifactRestorePath(
                attribute.RestorePath,
                workingDirectory);
            fileSystem.CreateDirectory(restorePath);
            await fileSystem.WriteAllBytesAsync(
                    fileSystem.Combine(restorePath, attribute.ArtifactName),
                    contents,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private static string ResolveArtifactRestorePath(
        string? restorePath,
        string workingDirectory)
    {
        var normalizedWorkingDirectory = Path.GetFullPath(workingDirectory);
        var configuredRestorePath = restorePath ?? normalizedWorkingDirectory;
        return Path.GetFullPath(
            Path.IsPathRooted(configuredRestorePath)
                ? configuredRestorePath
                : Path.Combine(normalizedWorkingDirectory, configuredRestorePath));
    }

    private static IModuleLogger GetModuleLogger(IServiceProvider services)
    {
        if (GeneratedModuleMetadata.TryGetRuntime(typeof(TModule), out var runtime))
        {
            return runtime.GetLogger(services);
        }

        return services.GetRequiredService<ModuleLogger<TModule>>();
    }

    private static void ValidateRequiredDependencyResults(
        IModule module,
        IServiceProvider services)
    {
        var registeredModuleTypes = services.GetServices<IModule>()
            .Select(static registeredModule => registeredModule.GetType())
            .ToArray();
        var dependencyRegistry = services.GetRequiredService<IModuleDependencyRegistry>();
        var metadataRegistry = services.GetRequiredService<IModuleMetadataRegistry>();
        var resultRegistry = services.GetRequiredService<IModuleResultRegistry>();
        var missingDependencies = ModuleDependencyResolver
            .GetAllDependencies(
                module,
                registeredModuleTypes,
                dependencyRegistry,
                metadataRegistry)
            .Where(static dependency => !dependency.Optional)
            .Select(static dependency => dependency.DependencyType)
            .Distinct()
            .Where(dependencyType => resultRegistry.GetResult(dependencyType) is null)
            .Select(static dependencyType => dependencyType.Name)
            .Order(StringComparer.Ordinal)
            .ToArray();

        if (missingDependencies.Length > 0)
        {
            throw new InvalidOperationException(
                $"Required dependency results must be seeded before testing {typeof(TModule).Name}: "
                + string.Join(", ", missingDependencies)
                + ". Call WithDependencyResult for each dependency.");
        }
    }

    internal sealed record ExecutionOutcome(
        IModuleResult Result,
        IReadOnlyList<RecordedCommand> Commands,
        IFileSystemProvider FileSystem);

    private interface IDependencySeed
    {
        void Apply(IServiceProvider services);
    }

    private sealed class DependencySeed<TDependency, TResult>(TResult value) : IDependencySeed
        where TDependency : Module<TResult>
    {
        public void Apply(IServiceProvider services)
        {
            var module = services.GetServices<IModule>()
                .OfType<TDependency>()
                .Single();
            var now = DateTimeOffset.UtcNow;
            var result = new ModuleResult<TResult>.Success(value)
            {
                Name = typeof(TDependency).Name,
                TypeName = typeof(TDependency).FullName,
                Duration = TimeSpan.Zero,
                StartTime = now,
                EndTime = now,
                Status = ModuleStatus.Succeeded,
                ModuleType = typeof(TDependency),
            };

            module.AsInternal().TrySetDistributedResult(result);
            services.GetRequiredService<IModuleResultRegistry>()
                .RegisterResult(typeof(TDependency), result);
        }
    }
}

/// <summary>
/// Configures strongly typed isolated execution of one module.
/// </summary>
/// <typeparam name="TModule">The module to execute.</typeparam>
/// <typeparam name="TResult">The module value type.</typeparam>
public sealed class ModuleTestBuilder<TModule, TResult> : ModuleTestBuilder<TModule>
    where TModule : Module<TResult>
{
    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithService{TService}(TService)"/>
    public new ModuleTestBuilder<TModule, TResult> WithService<TService>(TService service)
        where TService : class
    {
        base.WithService(service);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithDependencyResult{TDependency,TDependencyResult}(TDependencyResult)"/>
    public new ModuleTestBuilder<TModule, TResult> WithDependencyResult<TDependency, TDependencyResult>(
        TDependencyResult value)
        where TDependency : Module<TDependencyResult>
    {
        base.WithDependencyResult<TDependency, TDependencyResult>(value);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithArtifact{TProducer}(string,string)"/>
    public new ModuleTestBuilder<TModule, TResult> WithArtifact<TProducer>(
        string artifactName,
        string contents)
        where TProducer : class, IModule
    {
        base.WithArtifact<TProducer>(artifactName, contents);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithArtifact{TProducer}(string,byte[])"/>
    public new ModuleTestBuilder<TModule, TResult> WithArtifact<TProducer>(
        string artifactName,
        byte[] contents)
        where TProducer : class, IModule
    {
        base.WithArtifact<TProducer>(artifactName, contents);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.InterceptCommands(Func{CommandInvocation,CommandResult})"/>
    public new ModuleTestBuilder<TModule, TResult> InterceptCommands(
        Func<CommandInvocation, CommandResult> handler)
    {
        base.InterceptCommands(handler);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.InterceptCommands(Func{CommandInvocation,CancellationToken,ValueTask{CommandResult}})"/>
    public new ModuleTestBuilder<TModule, TResult> InterceptCommands(
        Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> handler)
    {
        base.InterceptCommands(handler);
        return this;
    }

    /// <summary>
    /// Executes the module in an isolated test pipeline.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The captured strongly typed module run.</returns>
    public new async Task<ModuleTestRun<TResult>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var execution = await ExecuteCoreAsync(cancellationToken).ConfigureAwait(false);
        return new ModuleTestRun<TResult>(
            (ModuleResult<TResult>) execution.Result,
            execution.Commands,
            execution.FileSystem);
    }
}
