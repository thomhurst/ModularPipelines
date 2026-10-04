using ModularPipelines.Context;
using Mediator;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
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
    private readonly Dictionary<string, byte[]> _fileSeeds = new(StringComparer.Ordinal);
    private Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>>? _commandHandler;

    /// <summary>Configures the pipeline after harness defaults and before building it.</summary>
    /// <param name="configure">The callback, invoked for each execution in registration order.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> ConfigurePipeline(Action<PipelineBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        _registrations.Add(configure);
        return this;
    }

    /// <summary>Seeds a UTF-8 file before pipeline service initialization and module execution.</summary>
    /// <param name="path">An absolute path or a path relative to the pipeline working directory.</param>
    /// <param name="contents">The file contents.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithFile(string path, string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);
        return WithFile(path, Encoding.UTF8.GetBytes(contents));
    }

    /// <summary>Seeds a binary file before pipeline service initialization, creating parent directories.</summary>
    /// <param name="path">An absolute path or a path relative to the pipeline working directory.</param>
    /// <param name="contents">The file contents, copied when registered.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithFile(string path, byte[] contents)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(contents);
        _fileSeeds[path] = contents.ToArray();
        return this;
    }

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
        _dependencySeeds.Add(new DependencySeed<TDependency, TResult>(
            context => ModuleResult<TResult>.CreateSuccess(value, context), ModuleStatus.Succeeded));
        return this;
    }

    /// <summary>Seeds a failed dependency without executing it.</summary>
    /// <typeparam name="TDependency">The dependency module.</typeparam>
    /// <typeparam name="TResult">The dependency value type.</typeparam>
    /// <param name="exception">The dependency failure.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithDependencyFailure<TDependency, TResult>(Exception exception)
        where TDependency : Module<TResult>
    {
        ArgumentNullException.ThrowIfNull(exception);
        _registrations.Add(static builder => builder.AddModule<TDependency>());
        _dependencySeeds.Add(new DependencySeed<TDependency, TResult>(
            context => ModuleResult<TResult>.CreateFailure(exception, context), ModuleStatus.Failed));
        return this;
    }

    /// <summary>Seeds a skipped dependency without executing it.</summary>
    /// <typeparam name="TDependency">The dependency module.</typeparam>
    /// <typeparam name="TResult">The dependency value type.</typeparam>
    /// <param name="reason">The reason the dependency was skipped.</param>
    /// <returns>This builder.</returns>
    public ModuleTestBuilder<TModule> WithSkippedDependency<TDependency, TResult>(string reason)
        where TDependency : Module<TResult>
    {
        ArgumentNullException.ThrowIfNull(reason);
        _registrations.Add(static builder => builder.AddModule<TDependency>());
        _dependencySeeds.Add(new DependencySeed<TDependency, TResult>(
            context => ModuleResult<TResult>.CreateSkipped(SkipDecision.Skip(reason), context), ModuleStatus.Skipped));
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

        var pipeline = await builder.BuildAsync(services => SeedFilesAsync(services, cancellationToken)).ConfigureAwait(false);
        await using var pipelineLifetime = pipeline.ConfigureAwait(false);

        var pipelineContext = pipeline.Services.GetRequiredService<IPipelineContext>();
        var effectiveFileSystem = pipeline.Services.GetRequiredService<IFileSystemProvider>();
        foreach (var dependencySeed in _dependencySeeds)
        {
            dependencySeed.Apply(pipeline.Services);
        }

        var module = pipeline.Services.GetServices<IModule>()
            .OfType<TModule>()
            .Single();
        var dependencies = ValidateRequiredDependencyResults(module, pipeline.Services);
        var executionContext = ExecutionContextFactory.Create(module, typeof(TModule));
        var dependencyFailure = ApplyDependencyOutcomes(module, dependencies, pipeline.Services, executionContext);
        if (dependencyFailure is not null)
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
            return new ExecutionOutcome(dependencyFailure, recorder.Commands, effectiveFileSystem);
        }

        if (cancellationToken.CanBeCanceled)
        {
            var originalCancellationTokenSource =
                executionContext.ModuleCancellationTokenSource;
            executionContext.ModuleCancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            originalCancellationTokenSource.Dispose();
        }

        var logger = GetModuleLogger(pipeline.Services);
        var mediator = pipeline.Services.GetRequiredKeyedService<IMediator>(typeof(global::Mediator.Mediator));
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

    private async Task SeedFilesAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var context = services.GetRequiredService<IPipelineContext>();
        var fileSystem = services.GetRequiredService<IFileSystemProvider>();
        foreach (var (path, contents) in _fileSeeds)
        {
            var absolutePath = context.Files.GetFile(path).Path;
            var directory = Path.GetDirectoryName(absolutePath);
            if (!string.IsNullOrEmpty(directory))
            {
                fileSystem.CreateDirectory(directory);
            }

            await fileSystem.WriteAllBytesAsync(absolutePath, contents.ToArray(), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private static IModuleResult? ApplyDependencyOutcomes(
        IModule module,
        (Type DependencyType, bool Optional)[] dependencies,
        IServiceProvider services,
        ModuleExecutionContext executionContext)
    {
        var registry = services.GetRequiredService<IModuleResultRegistry>();
        var outcomes = dependencies.Select(dependency => (
                dependency.DependencyType,
                dependency.Optional,
                Result: registry.GetResult(dependency.DependencyType)))
            .ToArray();

        if (!module.Configuration.AlwaysRun)
        {
            var failed = outcomes.FirstOrDefault(outcome => outcome.Result?.Status is
                ModuleStatus.Failed or ModuleStatus.TimedOut or ModuleStatus.Canceled or ModuleStatus.DependencyFailed);
            if (failed.Result is not null)
            {
                var dependency = services.GetServices<IModule>().Single(candidate => candidate.GetType() == failed.DependencyType);
                var exception = new DependencyFailedException(failed.Result.ExceptionOrDefault!, dependency);
                services.GetRequiredService<IModuleResultRegistrar>()
                    .RegisterDependencyFailedResult(module, typeof(TModule), exception);
                return registry.GetResult(typeof(TModule));
            }
        }

        var skipped = outcomes
            .Where(outcome => !outcome.Optional && outcome.Result?.Status == ModuleStatus.Skipped)
            .OrderBy(outcome => outcome.DependencyType.FullName, StringComparer.Ordinal)
            .Select(outcome => (outcome.DependencyType, outcome.Result!.SkipDecisionOrDefault))
            .ToArray();
        if (skipped.Length > 0)
        {
            executionContext.SkipResult = DependencySkipDecisionFactory.Create(skipped);
        }

        return null;
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

    private static (Type DependencyType, bool Optional)[] ValidateRequiredDependencyResults(
        IModule module,
        IServiceProvider services)
    {
        var registeredModuleTypes = services.GetServices<IModule>()
            .Select(static registeredModule => registeredModule.GetType())
            .ToArray();
        var dependencyRegistry = services.GetRequiredService<IModuleDependencyRegistry>();
        var metadataRegistry = services.GetRequiredService<IModuleMetadataRegistry>();
        var resultRegistry = services.GetRequiredService<IModuleResultRegistry>();
        var dependencies = ModuleDependencyResolver
            .GetAllDependencies(
                module,
                registeredModuleTypes,
                dependencyRegistry,
                metadataRegistry)
            .Distinct()
            .ToArray();
        var missingDependencies = dependencies
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

        return dependencies;
    }

    internal sealed record ExecutionOutcome(
        IModuleResult Result,
        IReadOnlyList<RecordedCommand> Commands,
        IFileSystemProvider FileSystem);

    private interface IDependencySeed
    {
        void Apply(IServiceProvider services);
    }

    private sealed class DependencySeed<TDependency, TResult>(
        Func<ModuleExecutionContext, ModuleResult<TResult>> createResult,
        ModuleStatus status) : IDependencySeed
        where TDependency : Module<TResult>
    {
        public void Apply(IServiceProvider services)
        {
            var module = services.GetServices<IModule>()
                .OfType<TDependency>()
                .Single();
            var now = DateTimeOffset.UtcNow;
            var context = new ModuleExecutionContext(module, typeof(TDependency))
            {
                Duration = TimeSpan.Zero,
                StartTime = now,
                EndTime = now,
                Status = status,
            };
            using var cancellation = context.ModuleCancellationTokenSource;
            var result = createResult(context);

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
    /// <inheritdoc cref="ModuleTestBuilder{TModule}.ConfigurePipeline"/>
    public new ModuleTestBuilder<TModule, TResult> ConfigurePipeline(Action<PipelineBuilder> configure)
    {
        base.ConfigurePipeline(configure);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithFile(string,string)"/>
    public new ModuleTestBuilder<TModule, TResult> WithFile(string path, string contents)
    {
        base.WithFile(path, contents);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithFile(string,byte[])"/>
    public new ModuleTestBuilder<TModule, TResult> WithFile(string path, byte[] contents)
    {
        base.WithFile(path, contents);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithDependencyFailure{TDependency,TDependencyResult}"/>
    public new ModuleTestBuilder<TModule, TResult> WithDependencyFailure<TDependency, TDependencyResult>(Exception exception)
        where TDependency : Module<TDependencyResult>
    {
        base.WithDependencyFailure<TDependency, TDependencyResult>(exception);
        return this;
    }

    /// <inheritdoc cref="ModuleTestBuilder{TModule}.WithSkippedDependency{TDependency,TDependencyResult}"/>
    public new ModuleTestBuilder<TModule, TResult> WithSkippedDependency<TDependency, TDependencyResult>(string reason)
        where TDependency : Module<TDependencyResult>
    {
        base.WithSkippedDependency<TDependency, TDependencyResult>(reason);
        return this;
    }

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
