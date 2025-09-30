using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Engine;

/// <summary>
/// Orchestrates distributed execution of modules across multiple nodes.
/// </summary>
internal sealed class DistributedModuleExecutor
{
    private readonly IDistributedScheduler _scheduler;
    private readonly IResultCache _resultCache;
    private readonly ILogger<DistributedModuleExecutor> _logger;

    public DistributedModuleExecutor(
        IDistributedScheduler scheduler,
        IResultCache resultCache,
        ILogger<DistributedModuleExecutor> logger)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _resultCache = resultCache ?? throw new ArgumentNullException(nameof(resultCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executes modules across distributed nodes according to an execution plan.
    /// </summary>
    /// <param name="modules">The modules to execute.</param>
    /// <param name="availableNodes">The available execution nodes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ExecuteAsync(
        IReadOnlyList<ModuleBase> modules,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(availableNodes);

        _logger.LogInformation(
            "Starting distributed execution of {ModuleCount} modules across {NodeCount} nodes",
            modules.Count,
            availableNodes.Count);

        // Create execution plan
        var plan = await _scheduler.CreateExecutionPlanAsync(modules, availableNodes, cancellationToken);

        // Execute modules wave by wave
        var completedModules = new ConcurrentDictionary<Type, IModuleResult>();

        foreach (var wave in plan.ExecutionWaves)
        {
            _logger.LogInformation(
                "Executing wave with {ModuleCount} modules",
                wave.Count);

            await ExecuteWaveAsync(wave, plan.ModuleAssignments, completedModules, availableNodes, cancellationToken);
        }

        _logger.LogInformation(
            "Completed distributed execution of all {ModuleCount} modules",
            modules.Count);
    }

    private async Task ExecuteWaveAsync(
        IReadOnlyList<ModuleBase> wave,
        IReadOnlyDictionary<ModuleBase, IExecutionNode> assignments,
        ConcurrentDictionary<Type, IModuleResult> completedModules,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken)
    {
        var moduleTasks = wave.Select(module =>
            ExecuteModuleAsync(module, assignments, completedModules, availableNodes, cancellationToken));

        await Task.WhenAll(moduleTasks);
    }

    private async Task ExecuteModuleAsync(
        ModuleBase module,
        IReadOnlyDictionary<ModuleBase, IExecutionNode> assignments,
        ConcurrentDictionary<Type, IModuleResult> completedModules,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken)
    {
        var moduleType = module.GetType();

        try
        {
            // Check if result is already cached
            if (await _resultCache.ContainsResultAsync(moduleType, cancellationToken))
            {
                var cachedResult = await _resultCache.GetResultAsync(moduleType, cancellationToken);
                if (cachedResult != null)
                {
                    completedModules[moduleType] = cachedResult;
                    _logger.LogInformation(
                        "Module {ModuleType} result retrieved from cache",
                        moduleType.Name);
                    return;
                }
            }

            // Get dependency results
            var dependencyResults = await GetDependencyResultsAsync(module, completedModules, cancellationToken);

            // Get assigned node
            if (!assignments.TryGetValue(module, out var assignedNode))
            {
                throw new InvalidOperationException(
                    $"No node assignment found for module {moduleType.Name}");
            }

            // Check if assigned node is still available
            if (!assignedNode.CanExecute(module))
            {
                _logger.LogWarning(
                    "Assigned node {NodeId} cannot execute module {ModuleType}. Attempting to reschedule.",
                    assignedNode.NodeId,
                    moduleType.Name);

                var newNode = await _scheduler.RescheduleModuleAsync(module, availableNodes, cancellationToken);

                if (newNode == null)
                {
                    throw new InvalidOperationException(
                        $"Unable to reschedule module {moduleType.Name}. No suitable nodes available.");
                }

                assignedNode = newNode;
            }

            _logger.LogInformation(
                "Executing module {ModuleType} on node {NodeId}",
                moduleType.Name,
                assignedNode.NodeId);

            // Execute module on assigned node
            var result = await assignedNode.ExecuteAsync(module, dependencyResults, cancellationToken);

            // Cache the result
            await _resultCache.SetResultAsync(moduleType, result, cancellationToken);

            // Add to completed modules
            completedModules[moduleType] = result;

            _logger.LogInformation(
                "Module {ModuleType} completed successfully on node {NodeId}. Duration: {Duration}",
                moduleType.Name,
                assignedNode.NodeId,
                result.ModuleDuration);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to execute module {ModuleType}",
                moduleType.Name);

            // Create failure result
            var failureResult = CreateFailureResult(module, ex);
            completedModules[moduleType] = failureResult;

            // Still cache the failure to prevent retries
            await _resultCache.SetResultAsync(moduleType, failureResult, cancellationToken);

            throw;
        }
    }

    private async Task<IReadOnlyDictionary<Type, IModuleResult>> GetDependencyResultsAsync(
        ModuleBase module,
        ConcurrentDictionary<Type, IModuleResult> completedModules,
        CancellationToken cancellationToken)
    {
        var dependencyResults = new Dictionary<Type, IModuleResult>();
        var dependencies = module.GetModuleDependencies();

        foreach (var (dependencyType, _) in dependencies)
        {
            // Try to get from completed modules first
            if (completedModules.TryGetValue(dependencyType, out var result))
            {
                dependencyResults[dependencyType] = result;
                continue;
            }

            // Try to get from cache
            result = await _resultCache.GetResultAsync(dependencyType, cancellationToken);

            if (result != null)
            {
                dependencyResults[dependencyType] = result;
                completedModules[dependencyType] = result;
            }
            else
            {
                _logger.LogWarning(
                    "Dependency {DependencyType} for module {ModuleType} not found in completed modules or cache",
                    dependencyType.Name,
                    module.GetType().Name);
            }
        }

        return dependencyResults;
    }

    private IModuleResult CreateFailureResult(ModuleBase module, Exception exception)
    {
        // Create a failure result using reflection to match the module's result type
        var moduleType = module.GetType();
        var baseType = moduleType.BaseType;

        while (baseType != null && !baseType.IsGenericType)
        {
            baseType = baseType.BaseType;
        }

        if (baseType?.GetGenericTypeDefinition() == typeof(Module<>))
        {
            var resultType = baseType.GetGenericArguments()[0];
            var moduleResultType = typeof(ModuleResult<>).MakeGenericType(resultType);

            // Use internal constructor: ModuleResult(Exception exception, ModuleBase module)
            var constructor = moduleResultType.GetConstructor(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new[] { typeof(Exception), typeof(ModuleBase) },
                null);

            if (constructor != null)
            {
                var result = constructor.Invoke(new object[] { exception, module }) as IModuleResult;
                if (result != null)
                {
                    return result;
                }
            }
        }

        // Fallback: create a basic ModuleResult
        var fallbackConstructor = typeof(ModuleResult).GetConstructor(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            new[] { typeof(Exception), typeof(ModuleBase) },
            null);

        if (fallbackConstructor != null)
        {
            return (IModuleResult)fallbackConstructor.Invoke(new object[] { exception, module });
        }

        throw new InvalidOperationException("Unable to create failure result");
    }
}
