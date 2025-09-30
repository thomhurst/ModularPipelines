using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Options;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Engine;

/// <summary>
/// Schedules modules across distributed execution nodes using a load-balanced algorithm.
/// </summary>
internal sealed class DistributedScheduler : IDistributedScheduler
{
    private readonly ILogger<DistributedScheduler> _logger;
    private readonly DistributedPipelineOptions _options;

    public DistributedScheduler(
        ILogger<DistributedScheduler> logger,
        IOptions<DistributedPipelineOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public Task<DistributedExecutionPlan> CreateExecutionPlanAsync(
        IReadOnlyList<ModuleBase> modules,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(availableNodes);

        if (availableNodes.Count == 0)
        {
            throw new InvalidOperationException("No execution nodes available");
        }

        _logger.LogInformation(
            "Creating execution plan for {ModuleCount} modules across {NodeCount} nodes",
            modules.Count,
            availableNodes.Count);

        // Build dependency graph
        var dependencyMap = BuildDependencyMap(modules);

        // Create execution waves (modules that can run in parallel)
        var executionWaves = CreateExecutionWaves(modules, dependencyMap);

        // Assign modules to nodes
        var assignments = AssignModulesToNodes(modules, availableNodes, dependencyMap);

        var plan = new DistributedExecutionPlan
        {
            ModuleAssignments = assignments,
            ExecutionWaves = executionWaves,
            EstimatedDuration = EstimateDuration(executionWaves, assignments),
        };

        _logger.LogInformation(
            "Execution plan created with {WaveCount} waves. Estimated duration: {Duration}",
            executionWaves.Count,
            plan.EstimatedDuration);

        return Task.FromResult(plan);
    }

    /// <inheritdoc />
    public Task<IExecutionNode?> RescheduleModuleAsync(
        ModuleBase module,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(availableNodes);

        if (availableNodes.Count == 0)
        {
            _logger.LogWarning(
                "No nodes available for rescheduling module {ModuleType}",
                module.GetType().Name);
            return Task.FromResult<IExecutionNode?>(null);
        }

        // Find the least loaded node that can execute this module
        var selectedNode = availableNodes
            .Where(node => node.CanExecute(module))
            .OrderBy(node => node.GetCurrentLoad())
            .FirstOrDefault();

        if (selectedNode != null)
        {
            _logger.LogInformation(
                "Rescheduled module {ModuleType} to node {NodeId}",
                module.GetType().Name,
                selectedNode.NodeId);
        }
        else
        {
            _logger.LogWarning(
                "No suitable node found for rescheduling module {ModuleType}",
                module.GetType().Name);
        }

        return Task.FromResult(selectedNode);
    }

    private Dictionary<ModuleBase, List<ModuleBase>> BuildDependencyMap(IReadOnlyList<ModuleBase> modules)
    {
        var dependencyMap = new Dictionary<ModuleBase, List<ModuleBase>>();

        foreach (var module in modules)
        {
            var dependencies = new List<ModuleBase>();
            var dependencyTypes = module.GetModuleDependencies()
                .Select(d => d.DependencyType)
                .ToList();

            foreach (var depType in dependencyTypes)
            {
                var dependency = modules.FirstOrDefault(m => m.GetType() == depType);
                if (dependency != null)
                {
                    dependencies.Add(dependency);
                }
            }

            dependencyMap[module] = dependencies;
        }

        return dependencyMap;
    }

    private List<IReadOnlyList<ModuleBase>> CreateExecutionWaves(
        IReadOnlyList<ModuleBase> modules,
        Dictionary<ModuleBase, List<ModuleBase>> dependencyMap)
    {
        var waves = new List<IReadOnlyList<ModuleBase>>();
        var remaining = new HashSet<ModuleBase>(modules);
        var completed = new HashSet<ModuleBase>();

        while (remaining.Count > 0)
        {
            // Find modules whose dependencies are all completed
            var currentWave = remaining
                .Where(m => dependencyMap[m].All(dep => completed.Contains(dep)))
                .ToList();

            if (currentWave.Count == 0)
            {
                // Circular dependency or other issue
                _logger.LogWarning(
                    "Unable to schedule {Count} remaining modules due to unresolved dependencies",
                    remaining.Count);
                break;
            }

            waves.Add(currentWave);

            foreach (var module in currentWave)
            {
                remaining.Remove(module);
                completed.Add(module);
            }

            _logger.LogDebug(
                "Wave {WaveNumber} contains {ModuleCount} modules",
                waves.Count,
                currentWave.Count);
        }

        return waves;
    }

    private IReadOnlyDictionary<ModuleBase, IExecutionNode> AssignModulesToNodes(
        IReadOnlyList<ModuleBase> modules,
        IReadOnlyList<IExecutionNode> availableNodes,
        Dictionary<ModuleBase, List<ModuleBase>> dependencyMap)
    {
        var assignments = new Dictionary<ModuleBase, IExecutionNode>();
        var nodeLoads = availableNodes.ToDictionary(n => n, n => n.GetCurrentLoad());

        // Separate local node if prefer local execution is enabled
        var localNode = availableNodes.FirstOrDefault(n => n is LocalExecutionNode);

        foreach (var module in modules)
        {
            IExecutionNode? selectedNode = null;

            // Check if module has special constraints
            var notInParallelAttr = module.GetType().GetCustomAttribute<NotInParallelAttribute>();
            if (notInParallelAttr != null)
            {
                // For now, execute NotInParallel modules locally
                // TODO: Implement distributed locking for NotInParallel across workers
                selectedNode = localNode;
                _logger.LogDebug(
                    "Module {ModuleType} has NotInParallel constraint, assigning to local node",
                    module.GetType().Name);
            }

            // Prefer local execution for modules with dependencies already on local node
            if (selectedNode == null && _options.PreferLocalExecution && localNode != null)
            {
                var dependencies = dependencyMap[module];
                if (dependencies.Any() &&
                    dependencies.All(dep => assignments.TryGetValue(dep, out var depNode) && depNode == localNode))
                {
                    selectedNode = localNode;
                    _logger.LogDebug(
                        "Module {ModuleType} dependencies are on local node, preferring local execution",
                        module.GetType().Name);
                }
            }

            // Otherwise, find the least loaded node that can execute this module
            if (selectedNode == null)
            {
                selectedNode = availableNodes
                    .Where(node => node.CanExecute(module))
                    .OrderBy(node => nodeLoads[node])
                    .FirstOrDefault();
            }

            if (selectedNode == null)
            {
                _logger.LogWarning(
                    "No suitable node found for module {ModuleType}",
                    module.GetType().Name);
                // Fallback to local node or first available node
                selectedNode = localNode ?? availableNodes.First();
            }

            assignments[module] = selectedNode;
            nodeLoads[selectedNode]++;

            _logger.LogDebug(
                "Assigned module {ModuleType} to node {NodeId}",
                module.GetType().Name,
                selectedNode.NodeId);
        }

        // Log assignment summary
        var summary = assignments
            .GroupBy(kvp => kvp.Value.NodeId)
            .Select(g => $"{g.Key}: {g.Count()} modules")
            .ToList();

        _logger.LogInformation(
            "Module assignment summary: {Summary}",
            string.Join(", ", summary));

        return assignments;
    }

    private TimeSpan EstimateDuration(
        List<IReadOnlyList<ModuleBase>> executionWaves,
        IReadOnlyDictionary<ModuleBase, IExecutionNode> assignments)
    {
        // Simple estimation: sum of wave durations
        // Each wave's duration is the max module duration in that wave
        var totalDuration = TimeSpan.Zero;

        foreach (var wave in executionWaves)
        {
            // Assume each module takes 30 seconds on average (placeholder)
            // In reality, this should use historical data or estimates
            var waveDuration = TimeSpan.FromSeconds(30);
            totalDuration += waveDuration;
        }

        return totalDuration;
    }
}
