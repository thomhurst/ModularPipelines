using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Registers skipped results for modules that were ignored via Category or RunCondition.
/// This ensures tests and other code can retrieve results for these modules.
/// If a history repository is configured and has a cached result, it will be used.
/// </summary>
internal class IgnoredModuleResultRegistrar : IIgnoredModuleResultRegistrar
{
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IModuleResultHistoryProvider _resultHistoryProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IOptions<DistributedOptions> _distributedOptions;
    private readonly RoleDetector _roleDetector;
    private readonly ILogger<IgnoredModuleResultRegistrar> _logger;

    public IgnoredModuleResultRegistrar(
        IModuleResultRegistry resultRegistry,
        IModuleResultHistoryProvider resultHistoryProvider,
        IPipelineContextProvider pipelineContextProvider,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IOptions<DistributedOptions> distributedOptions,
        RoleDetector roleDetector,
        ILogger<IgnoredModuleResultRegistrar> logger)
    {
        _resultRegistry = resultRegistry;
        _resultHistoryProvider = resultHistoryProvider;
        _pipelineContextProvider = pipelineContextProvider;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _distributedOptions = distributedOptions;
        _roleDetector = roleDetector;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<OrganizedModules> RegisterIgnoredModuleResultsAsync(OrganizedModules organizedModules)
    {
        if (IsDistributedWorker())
        {
            return organizedModules;
        }

        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var runnableModules = organizedModules.RunnableModules.ToList();
        var ignoredModules = organizedModules.IgnoredModules.ToList();
        var consumedArtifactProducerTypes = runnableModules
            .SelectMany(runnableModule => runnableModule.Module.GetType()
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>())
            .Select(attribute => attribute.ProducerModule)
            .ToHashSet();
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
            organizedModules.AllModules.ToArray(),
            runnableModules.Select(runnableModule => runnableModule.Module),
            ignoredModules,
            _dependencyRegistry,
            _metadataRegistry,
            async pendingIgnoredModules =>
            {
                foreach (var ignoredModule in pendingIgnoredModules)
                {
                    await RegisterIgnoredModuleResultAsync(
                            ignoredModule,
                            pipelineContext,
                            allowHistory: !consumedArtifactProducerTypes.Contains(
                                ignoredModule.Module.GetType()))
                        .ConfigureAwait(false);
                }
            },
            moduleType => _resultRegistry.GetResult(moduleType)?.ModuleStatus == Status.Skipped)
            .ConfigureAwait(false);
        var remainingModules = cascadeResult.RunnableModules.ToHashSet<IModule>(
            ReferenceEqualityComparer.Instance);

        return new OrganizedModules(
            runnableModules.Where(runnableModule => remainingModules.Contains(runnableModule.Module)).ToList(),
            cascadeResult.IgnoredModules);
    }

    private bool IsDistributedWorker()
    {
        var options = _distributedOptions.Value;
        return options.Enabled
               && options.TotalInstances > 1
               && _roleDetector.DetectRole() == DistributedRole.Worker;
    }

    private async Task RegisterIgnoredModuleResultAsync(
        IgnoredModule ignoredModule,
        IPipelineContext pipelineContext,
        bool allowHistory)
    {
        var module = ignoredModule.Module;
        var moduleType = module.GetType();
        var resultType = module.ResultType;

        var historicalResult = allowHistory
            ? await _resultHistoryProvider
                .TryGetAsync(module, pipelineContext)
                .ConfigureAwait(false)
            : null;
        if (historicalResult != null)
        {
            var usedHistoryResult = ModuleResultFactory.WithStatus(historicalResult, Status.UsedHistory);
            _logger.LogDebug("Using historical result for ignored module {ModuleName}",
                moduleType.Name);
            _resultRegistry.RegisterResult(moduleType, usedHistoryResult);

            SetModuleCompletionSource(module, resultType, usedHistoryResult);
            return;
        }

        _logger.LogDebug("Registering skipped result for ignored module {ModuleName}",
            moduleType.Name);

        // Create execution context with Skipped status using compiled delegate factory
        var executionContext = ExecutionContextFactory.Create(module, moduleType);
        executionContext.Status = Status.Skipped;
        executionContext.SkipResult = ignoredModule.SkipDecision;

        // Prefer generated typed metadata so Native AOT has compiled result and
        // completion-source adapters.
        var hasGeneratedRuntime = GeneratedModuleMetadata.TryGetRuntime(
            moduleType,
            out var runtime);
        var result = hasGeneratedRuntime
            ? runtime.CreateSkipped(executionContext)
            : ModuleResultFactory.CreateSkipped(resultType, executionContext);

        _resultRegistry.RegisterResult(moduleType, result);

        // Set the completion source so awaiting the module returns immediately
        if (hasGeneratedRuntime)
        {
            runtime.SetCompletionSource(module, result);
        }
        else
        {
            SetModuleCompletionSource(module, resultType, result);
        }
    }

    /// <summary>
    /// Sets the completion source on a module so that awaiting the module returns immediately.
    /// This is necessary for ignored modules so that dependent modules don't wait forever.
    /// </summary>
    private static void SetModuleCompletionSource(IModule module, Type resultType, IModuleResult result)
    {
        var setter = CompletionSourceSetterCache.GetOrCreate(resultType);
        setter(module, result);
    }
}

/// <summary>
/// Cache for compiled delegates that set the completion source on a module.
/// </summary>
internal static class CompletionSourceSetterCache
{
    private static readonly ConcurrentDictionary<Type, Action<IModule, IModuleResult>> Cache = new();

    /// <summary>
    /// Gets or creates a compiled delegate that sets the completion source on a module.
    /// </summary>
    public static Action<IModule, IModuleResult> GetOrCreate(Type resultType)
    {
        return Cache.GetOrAdd(resultType, CreateSetter);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "The dynamic completion-source setter is used by history paths that are unsupported in Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The dynamic completion-source setter is used by history paths that are unsupported when trimming.")]
    private static Action<IModule, IModuleResult> CreateSetter(Type resultType)
    {
        // Create compiled delegate for: ((Module<T>)module).CompletionSource.TrySetResult((ModuleResult<T>)result)
        var moduleType = typeof(Module<>).MakeGenericType(resultType);

        // Parameters
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var resultParam = Expression.Parameter(typeof(IModuleResult), "result");

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // Access CompletionSource property
        var completionSourceProp = moduleType.GetProperty("CompletionSource",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"CompletionSource property not found on {moduleType.Name}");
        var completionSource = Expression.Property(castModule, completionSourceProp);

        // Get the actual property type and its TrySetResult method
        // This ensures we use the correct generic type as declared on the property
        var completionSourceType = completionSourceProp.PropertyType;
        var moduleResultType = completionSourceType.GetGenericArguments()[0]; // ModuleResult<T>

        // Cast result to ModuleResult<T>
        var castResult = Expression.Convert(resultParam, moduleResultType);

        // Call TrySetResult using the method from the actual property type
        var trySetResultMethod = completionSourceType.GetMethod("TrySetResult")!;
        var callTrySetResult = Expression.Call(completionSource, trySetResultMethod, castResult);

        // Compile to Action<IModule, IModuleResult>
        var lambda = Expression.Lambda<Action<IModule, IModuleResult>>(callTrySetResult, moduleParam, resultParam);
        return lambda.Compile();
    }
}
