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
using ModularPipelines.Generated;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Registers skipped results for modules that were ignored via Category or RunCondition.
/// This ensures tests and other code can retrieve results for these modules.
/// If a history repository is configured and has a cached result, it will be used.
/// </summary>
internal class IgnoredModuleResultRegistrar(
    IModuleResultRegistry resultRegistry,
    IModuleResultHistoryProvider resultHistoryProvider,
    IPipelineContextProvider pipelineContextProvider,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IOptions<DistributedOptions> distributedOptions,
    RoleDetector roleDetector,
    ILogger<IgnoredModuleResultRegistrar> logger,
    ModulePlanningSkipEvaluator modulePlanningSkipEvaluator) : IIgnoredModuleResultRegistrar
{
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleResultHistoryProvider _resultHistoryProvider = resultHistoryProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider = pipelineContextProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry = metadataRegistry;
    private readonly IOptions<DistributedOptions> _distributedOptions = distributedOptions;
    private readonly RoleDetector _roleDetector = roleDetector;
    private readonly ILogger<IgnoredModuleResultRegistrar> _logger = logger;
    private readonly ModulePlanningSkipEvaluator _modulePlanningSkipEvaluator = modulePlanningSkipEvaluator;

    /// <inheritdoc />
    public async Task<OrganizedModules> RegisterIgnoredModuleResultsAsync(OrganizedModules organizedModules)
    {
        var resolution = await ResolveIgnoredModuleResultsCoreAsync(
                organizedModules,
                _dependencyRegistry,
                _metadataRegistry,
                registerResults: true,
                historyModules: null,
                CancellationToken.None)
            .ConfigureAwait(false);
        return resolution.OrganizedModules;
    }

    /// <inheritdoc />
    public Task<IgnoredModuleResolution> ResolveIgnoredModuleResultsAsync(
        OrganizedModules organizedModules,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule> historyModules,
        CancellationToken cancellationToken) =>
        ResolveIgnoredModuleResultsCoreAsync(
            organizedModules,
            dependencyRegistry,
            metadataRegistry,
            registerResults: false,
            historyModules,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlySet<Type>> ResolveHistoryModuleTypesAsync(
        IEnumerable<IModule> ignoredModules,
        IReadOnlyDictionary<IModule, IModule> historyModules,
        CancellationToken cancellationToken)
    {
        var usedHistoryModuleTypes = new HashSet<Type>();
        if (IsDistributedWorker())
        {
            return usedHistoryModuleTypes;
        }

        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        foreach (var module in ignoredModules.Distinct<IModule>(ReferenceEqualityComparer.Instance))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var historicalResult = await _resultHistoryProvider
                .TryGetAsync(historyModules[module], pipelineContext)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (historicalResult is not null)
            {
                usedHistoryModuleTypes.Add(module.GetType());
            }
        }

        return usedHistoryModuleTypes;
    }

    private async Task<IgnoredModuleResolution> ResolveIgnoredModuleResultsCoreAsync(
        OrganizedModules organizedModules,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        bool registerResults,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken)
    {
        if (IsDistributedWorker())
        {
            return new IgnoredModuleResolution(organizedModules, new HashSet<Type>());
        }

        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var runnableModules = organizedModules.RunnableModules.ToList();
        var ignoredModules = organizedModules.IgnoredModules.ToList();
        var allModules = organizedModules.AllModules.ToArray();
        var usedHistoryModuleTypes = new HashSet<Type>();
        var skippedModuleTypes = new HashSet<Type>();
        var availableModuleTypes = allModules
            .Select(module => module.GetType())
            .Distinct()
            .ToArray();
        var modulesByType = allModules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var historicalResults = new Dictionary<IModule, IModuleResult?>(
            ReferenceEqualityComparer.Instance);
        var planningSkipDecisions = new Dictionary<IModule, SkipDecision?>(
            ReferenceEqualityComparer.Instance);
        foreach (var ignoredModule in ignoredModules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            historicalResults[ignoredModule.Module] = await _resultHistoryProvider
                .TryGetAsync(
                    GetHistoryModule(ignoredModule.Module, historyModules),
                    pipelineContext)
                .ConfigureAwait(false);
        }

        var ignoredModuleTypes = ignoredModules
            .Select(ignoredModule => ignoredModule.Module.GetType())
            .ToHashSet();
        var ignoredModuleTypesWithoutHistory = ignoredModules
            .Where(ignoredModule => historicalResults[ignoredModule.Module] is null)
            .Select(ignoredModule => ignoredModule.Module.GetType())
            .ToHashSet();
        var consumedArtifactProducerTypes = await ResolveConsumedArtifactProducerTypesAsync(
                runnableModules,
                modulesByType,
                availableModuleTypes,
                ignoredModuleTypes,
                ignoredModuleTypesWithoutHistory,
                historicalResults,
                planningSkipDecisions,
                pipelineContext,
                dependencyRegistry,
                metadataRegistry,
                historyModules,
                cancellationToken)
            .ConfigureAwait(false);
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
            allModules,
            runnableModules.Select(runnableModule => runnableModule.Module),
            ignoredModules,
            dependencyRegistry,
            metadataRegistry,
            async pendingIgnoredModules =>
            {
                foreach (var ignoredModule in pendingIgnoredModules)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (!historicalResults.TryGetValue(ignoredModule.Module, out var historicalResult))
                    {
                        historicalResult = await _resultHistoryProvider
                            .TryGetAsync(
                                GetHistoryModule(ignoredModule.Module, historyModules),
                                pipelineContext)
                            .ConfigureAwait(false);
                        historicalResults[ignoredModule.Module] = historicalResult;
                    }

                    var result = CreateIgnoredModuleResult(
                        ignoredModule,
                        historicalResult,
                        allowHistory: !consumedArtifactProducerTypes.Contains(
                            ignoredModule.Module.GetType()));
                    cancellationToken.ThrowIfCancellationRequested();
                    var moduleType = ignoredModule.Module.GetType();
                    if (result.Status == ModuleStatus.RestoredFromHistory)
                    {
                        usedHistoryModuleTypes.Add(moduleType);
                    }
                    else
                    {
                        skippedModuleTypes.Add(moduleType);
                    }

                    if (registerResults)
                    {
                        _resultRegistry.RegisterResult(moduleType, result);
                        SetModuleCompletionSource(ignoredModule.Module, ignoredModule.Module.ResultType, result);
                    }
                }
            },
            skippedModuleTypes.Contains,
            cancellationToken)
            .ConfigureAwait(false);
        var remainingModules = cascadeResult.RunnableModules.ToHashSet<IModule>(
            ReferenceEqualityComparer.Instance);

        return new IgnoredModuleResolution(
            new OrganizedModules(
                [.. runnableModules.Where(runnableModule => remainingModules.Contains(runnableModule.Module))],
                cascadeResult.IgnoredModules),
            usedHistoryModuleTypes);
    }

    private async Task<HashSet<Type>> ResolveConsumedArtifactProducerTypesAsync(
        IReadOnlyList<RunnableModule> runnableModules,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IReadOnlyCollection<Type> availableModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypesWithoutHistory,
        IDictionary<IModule, IModuleResult?> historicalResults,
        IDictionary<IModule, SkipDecision?> planningSkipDecisions,
        IPipelineContext pipelineContext,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken)
    {
        return await ArtifactDemandPlanner.ResolveAsync(async currentDemand =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var nextConsumedArtifactProducerTypes = new HashSet<Type>();
            var unrecoverableIgnoredModuleTypes = ignoredModuleTypesWithoutHistory
                .Concat(currentDemand)
                .ToHashSet();
            foreach (var runnableModule in runnableModules)
            {
                var consumedProducerTypes = runnableModule.Module.GetType()
                    .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                    .Cast<ConsumesArtifactAttribute>()
                    .Select(attribute => attribute.ProducerModule)
                    .Where(ignoredModuleTypes.Contains)
                    .ToHashSet();
                if (consumedProducerTypes.Count == 0)
                {
                    continue;
                }

                var dependencyDemand = await GetRequiredDependencyDemandAsync(
                        runnableModule.Module,
                        modulesByType,
                        availableModuleTypes,
                        ignoredModuleTypes,
                        unrecoverableIgnoredModuleTypes,
                        consumedProducerTypes,
                        historicalResults,
                        planningSkipDecisions,
                        pipelineContext,
                        dependencyRegistry,
                        metadataRegistry,
                        historyModules,
                        cancellationToken)
                    .ConfigureAwait(false);
                if (dependencyDemand.IsUnrecoverable)
                {
                    continue;
                }

                if (dependencyDemand.HasPendingDependency)
                {
                    nextConsumedArtifactProducerTypes.UnionWith(consumedProducerTypes);
                    continue;
                }

                if (!planningSkipDecisions.TryGetValue(runnableModule.Module, out var skipDecision))
                {
                    skipDecision = await EvaluatePlanningSkipAsync(
                            runnableModule.Module,
                            metadataRegistry,
                            historyModules,
                            cancellationToken)
                        .ConfigureAwait(false);
                    planningSkipDecisions[runnableModule.Module] = skipDecision;
                }

                if (skipDecision?.ShouldSkip != true)
                {
                    nextConsumedArtifactProducerTypes.UnionWith(consumedProducerTypes);
                }
            }

            return nextConsumedArtifactProducerTypes;
        }).ConfigureAwait(false);
    }

    private async Task<RequiredDependencyDemand> GetRequiredDependencyDemandAsync(
        IModule module,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IReadOnlyCollection<Type> availableModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> unrecoverableIgnoredModuleTypes,
        IReadOnlySet<Type> consumedProducerTypes,
        IDictionary<IModule, IModuleResult?> historicalResults,
        IDictionary<IModule, SkipDecision?> planningSkipDecisions,
        IPipelineContext pipelineContext,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken)
    {
        return await GetRequiredDependencyDemandAsync(
                module,
                modulesByType,
                availableModuleTypes,
                ignoredModuleTypes,
                unrecoverableIgnoredModuleTypes,
                consumedProducerTypes,
                historicalResults,
                planningSkipDecisions,
                pipelineContext,
                dependencyRegistry,
                metadataRegistry,
                historyModules,
                cancellationToken,
                new HashSet<Type> { module.GetType() })
            .ConfigureAwait(false);
    }

    private async Task<RequiredDependencyDemand> GetRequiredDependencyDemandAsync(
        IModule module,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IReadOnlyCollection<Type> availableModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> unrecoverableIgnoredModuleTypes,
        IReadOnlySet<Type> consumedProducerTypes,
        IDictionary<IModule, IModuleResult?> historicalResults,
        IDictionary<IModule, SkipDecision?> planningSkipDecisions,
        IPipelineContext pipelineContext,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken,
        ISet<Type> visitedTypes)
    {
        var hasPendingDependency = false;
        var requiredDependencies = ModuleDependencyResolver
            .GetAllDependencies(
                module,
                availableModuleTypes,
                dependencyRegistry,
                metadataRegistry)
            .Where(dependency => !dependency.Optional)
            .Select(dependency => dependency.DependencyType);
        foreach (var dependencyType in requiredDependencies)
        {
            var dependencyDemand = await GetDependencyDemandAsync(
                    dependencyType,
                    modulesByType,
                    availableModuleTypes,
                    ignoredModuleTypes,
                    unrecoverableIgnoredModuleTypes,
                    consumedProducerTypes,
                    visitedTypes,
                    historicalResults,
                    planningSkipDecisions,
                    pipelineContext,
                    dependencyRegistry,
                    metadataRegistry,
                    historyModules,
                    cancellationToken)
                .ConfigureAwait(false);
            if (dependencyDemand.IsUnrecoverable)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: true,
                    HasPendingDependency: hasPendingDependency
                                          || dependencyDemand.HasPendingDependency);
            }

            hasPendingDependency |= dependencyDemand.HasPendingDependency;
        }

        return new RequiredDependencyDemand(
            IsUnrecoverable: false,
            HasPendingDependency: hasPendingDependency);
    }

    private async Task<RequiredDependencyDemand> GetDependencyDemandAsync(
        Type dependencyType,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IReadOnlyCollection<Type> availableModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> unrecoverableIgnoredModuleTypes,
        IReadOnlySet<Type> consumedProducerTypes,
        ISet<Type> visitedTypes,
        IDictionary<IModule, IModuleResult?> historicalResults,
        IDictionary<IModule, SkipDecision?> planningSkipDecisions,
        IPipelineContext pipelineContext,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken)
    {
        if (ignoredModuleTypes.Contains(dependencyType))
        {
            var isUnrecoverable = !consumedProducerTypes.Contains(dependencyType)
                                  && unrecoverableIgnoredModuleTypes.Contains(dependencyType);
            return new RequiredDependencyDemand(isUnrecoverable, HasPendingDependency: false);
        }

        if (!modulesByType.TryGetValue(dependencyType, out var dependencyModule)
            || !visitedTypes.Add(dependencyType))
        {
            return default;
        }

        try
        {
            var transitiveDemand = await GetRequiredDependencyDemandAsync(
                    dependencyModule,
                    modulesByType,
                    availableModuleTypes,
                    ignoredModuleTypes,
                    unrecoverableIgnoredModuleTypes,
                    consumedProducerTypes,
                    historicalResults,
                    planningSkipDecisions,
                    pipelineContext,
                    dependencyRegistry,
                    metadataRegistry,
                    historyModules,
                    cancellationToken,
                    visitedTypes)
                .ConfigureAwait(false);
            if (transitiveDemand.IsUnrecoverable)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: true,
                    HasPendingDependency: true);
            }

            if (transitiveDemand.HasPendingDependency)
            {
                return transitiveDemand;
            }

            if (!planningSkipDecisions.TryGetValue(dependencyModule, out var skipDecision))
            {
                skipDecision = await EvaluatePlanningSkipAsync(
                        dependencyModule,
                        metadataRegistry,
                        historyModules,
                        cancellationToken)
                    .ConfigureAwait(false);
                planningSkipDecisions[dependencyModule] = skipDecision;
            }

            if (skipDecision?.ShouldSkip != true)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: false,
                    HasPendingDependency: true);
            }

            if (!historicalResults.TryGetValue(dependencyModule, out var historicalResult))
            {
                historicalResult = await _resultHistoryProvider
                    .TryGetAsync(
                        GetHistoryModule(dependencyModule, historyModules),
                        pipelineContext)
                    .ConfigureAwait(false);
                historicalResults[dependencyModule] = historicalResult;
            }

            return new RequiredDependencyDemand(
                IsUnrecoverable: historicalResult is null,
                HasPendingDependency: false);
        }
        finally
        {
            visitedTypes.Remove(dependencyType);
        }
    }

    private readonly record struct RequiredDependencyDemand(
        bool IsUnrecoverable,
        bool HasPendingDependency);

    private Task<SkipDecision?> EvaluatePlanningSkipAsync(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule>? historyModules,
        CancellationToken cancellationToken) =>
        historyModules is null
            ? _modulePlanningSkipEvaluator.EvaluateAsync(module, cancellationToken)
            : _modulePlanningSkipEvaluator.EvaluateGraphSafeAsync(
                module,
                metadataRegistry,
                cancellationToken);

    private static IModule GetHistoryModule(
        IModule module,
        IReadOnlyDictionary<IModule, IModule>? historyModules) =>
        historyModules is null ? module : historyModules[module];

    private bool IsDistributedWorker()
    {
        var options = _distributedOptions.Value;
        return options.Enabled
               && options.TotalInstances > 1
               && _roleDetector.DetectRole() == DistributedRole.Worker;
    }

    private IModuleResult CreateIgnoredModuleResult(
        IgnoredModule ignoredModule,
        IModuleResult? historicalResult,
        bool allowHistory)
    {
        var module = ignoredModule.Module;
        var moduleType = module.GetType();
        var resultType = module.ResultType;

        if (allowHistory && historicalResult != null)
        {
            var usedHistoryResult = ModuleResultFactory.WithStatus(historicalResult, ModuleStatus.RestoredFromHistory);
            _logger.LogDebug("Using historical result for ignored module {ModuleName}",
                moduleType.Name);
            return usedHistoryResult;
        }

        _logger.LogDebug("Registering skipped result for ignored module {ModuleName}",
            moduleType.Name);

        // Create execution context with Skipped status using compiled delegate factory
        var executionContext = ExecutionContextFactory.Create(module, moduleType);
        executionContext.Status = ModuleStatus.Skipped;
        executionContext.SkipResult = ignoredModule.SkipDecision;

        // Prefer generated typed metadata so Native AOT has compiled result and
        // completion-source adapters.
        var hasGeneratedRuntime = GeneratedModuleMetadata.TryGetRuntime(
            moduleType,
            out var runtime);
        try
        {
            return hasGeneratedRuntime
                ? runtime.CreateSkipped(executionContext)
                : ModuleResultFactory.CreateSkipped(resultType, executionContext);
        }
        finally
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
        }
    }

    /// <summary>
    /// Sets the completion source on a module so that awaiting the module returns immediately.
    /// This is necessary for ignored modules so that dependent modules don't wait forever.
    /// </summary>
    private static void SetModuleCompletionSource(IModule module, Type resultType, IModuleResult result)
    {
        if (GeneratedModuleMetadata.TryGetRuntime(module.GetType(), out var runtime))
        {
            runtime.SetCompletionSource(module, result);
            return;
        }

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
