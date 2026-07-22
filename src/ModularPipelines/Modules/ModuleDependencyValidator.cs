using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;

namespace ModularPipelines.Modules;

/// <summary>
/// Validates module dependencies at registration time, before the pipeline executes.
/// This catches configuration errors early, preventing runtime failures.
/// </summary>
public static class ModuleDependencyValidator
{
    /// <summary>
    /// Validates registered module instances, including dependencies declared through fluent configuration.
    /// </summary>
    /// <param name="registeredModules">The registered module instances.</param>
    public static void Validate(IEnumerable<IModule> registeredModules)
    {
        var modulesByType = registeredModules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var moduleTypes = modulesByType.Keys.ToHashSet();
        if (moduleTypes.Count == 0)
        {
            return;
        }

        var dependenciesByModule = modulesByType.ToDictionary(
            pair => pair.Key,
            pair => GetConfiguredDependencies(pair.Value, moduleTypes));

        foreach (var (moduleType, dependencies) in dependenciesByModule)
        {
            foreach (var (dependencyType, optional) in dependencies)
            {
                if (dependencyType == moduleType)
                {
                    throw new ModuleReferencingSelfException(
                        $"Module '{moduleType.Name}' cannot reference itself. " +
                        "A module cannot depend on its own result.");
                }

                if (!optional && !moduleTypes.Contains(dependencyType))
                {
                    throw new ModuleNotRegisteredException(
                        $"Module '{moduleType.Name}' requires '{dependencyType.Name}', " +
                        $"but '{dependencyType.Name}' is not registered and could not be auto-registered. " +
                        "Either register the dependency module or make the dependency optional.", null);
                }
            }
        }

        var dependencyGraph = dependenciesByModule.ToDictionary(
            pair => pair.Key,
            pair => pair.Value
                .Where(dependency => moduleTypes.Contains(dependency.DependencyType))
                .Select(dependency => dependency.DependencyType)
                .ToHashSet());
        ValidateCircularDependencies(dependencyGraph);
    }

    /// <summary>
    /// Validates all registered module dependencies.
    /// </summary>
    /// <param name="registeredModuleTypes">The types of all registered modules.</param>
    /// <exception cref="ModuleReferencingSelfException">
    /// Thrown when a module depends on itself.
    /// </exception>
    /// <exception cref="DependencyCollisionException">
    /// Thrown when circular dependencies are detected.
    /// </exception>
    /// <exception cref="ModuleNotRegisteredException">
    /// Thrown when a required dependency is not registered.
    /// </exception>
    public static void Validate(IEnumerable<Type> registeredModuleTypes)
    {
        var moduleTypes = registeredModuleTypes.ToHashSet();

        if (moduleTypes.Count == 0)
        {
            return;
        }

        ValidateSelfReferences(moduleTypes);
        ValidateMissingDependencies(moduleTypes);
        ValidateCircularDependencies(moduleTypes);
    }

    private static HashSet<(Type DependencyType, bool Optional)> GetConfiguredDependencies(
        IModule module,
        HashSet<Type> moduleTypes)
    {
        return [.. ModuleDependencyResolver.GetDependencies(module.GetType(), moduleTypes)
            .Concat(module.Configuration.Dependencies.Select(dependency =>
                (DependencyType: dependency.ModuleType, Optional: dependency.IsOptional)))
            .GroupBy(dependency => dependency.DependencyType)
            .Select(group => (
                DependencyType: group.Key,
                Optional: group.All(dependency => dependency.Optional)))];
    }

    /// <summary>
    /// Validates that no module references itself.
    /// </summary>
    private static void ValidateSelfReferences(HashSet<Type> moduleTypes)
    {
        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes);

            foreach (var (dependencyType, _) in dependencies)
            {
                if (dependencyType == moduleType)
                {
                    throw new ModuleReferencingSelfException(
                        $"Module '{moduleType.Name}' cannot reference itself. " +
                        "A module cannot depend on its own result.");
                }
            }
        }
    }

    /// <summary>
    /// Validates that all required (non-optional) dependencies are registered.
    /// </summary>
    private static void ValidateMissingDependencies(HashSet<Type> moduleTypes)
    {
        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes);

            foreach (var (dependencyType, optional) in dependencies)
            {
                // Skip validation for optional dependencies
                if (optional)
                {
                    continue;
                }

                if (!moduleTypes.Contains(dependencyType))
                {
                    throw new ModuleNotRegisteredException(
                        $"Module '{moduleType.Name}' requires '{dependencyType.Name}', " +
                        $"but '{dependencyType.Name}' is not registered and could not be auto-registered. " +
                        "Either register the dependency module or use [DependsOn<T>(Optional = true)] if the dependency is optional.", null);
                }
            }
        }
    }

    /// <summary>
    /// Validates that there are no circular dependencies between modules.
    /// </summary>
    private static void ValidateCircularDependencies(HashSet<Type> moduleTypes)
    {
        // Build dependency graph
        var dependencyGraph = new Dictionary<Type, HashSet<Type>>();

        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes)
                .Where(d => moduleTypes.Contains(d.DependencyType))
                .Select(d => d.DependencyType)
                .ToHashSet();

            dependencyGraph[moduleType] = dependencies;
        }

        ValidateCircularDependencies(dependencyGraph);
    }

    /// <summary>
    /// Validates a dependency graph that may have been extended at runtime.
    /// </summary>
    /// <param name="dependencyGraph">The declared dependencies keyed by registered module type.</param>
    /// <exception cref="DependencyCollisionException">Thrown when circular dependencies are detected.</exception>
    internal static void ValidateCircularDependencies(IReadOnlyDictionary<Type, HashSet<Type>> dependencyGraph)
    {
        // Detect cycles using DFS with coloring
        // White (0) = not visited, Gray (1) = in current path, Black (2) = fully processed
        var colors = new Dictionary<Type, int>();
        var parent = new Dictionary<Type, Type?>();

        foreach (var moduleType in dependencyGraph.Keys)
        {
            colors[moduleType] = 0;
            parent[moduleType] = null;
        }

        foreach (var moduleType in dependencyGraph.Keys)
        {
            if (colors[moduleType] == 0)
            {
                var cycleStart = DetectCycleDfs(moduleType, dependencyGraph, colors, parent);
                if (cycleStart != null)
                {
                    var cycle = BuildCyclePath(cycleStart, parent);
                    var formattedArray = cycle.Select(t => t.Name).ToArray();

                    // Format with bold markers on first and last to match existing behavior
                    formattedArray[0] = $"**{formattedArray[0]}**";
                    formattedArray[^1] = $"**{formattedArray[^1]}**";

                    var cycleDescription = string.Join(" -> ", formattedArray);

                    throw new DependencyCollisionException(
                        $"Dependency collision detected: {cycleDescription}");
                }
            }
        }
    }

    /// <summary>
    /// Performs DFS to detect cycles, returning the start of a cycle if found.
    /// </summary>
    private static Type? DetectCycleDfs(
        Type current,
        IReadOnlyDictionary<Type, HashSet<Type>> graph,
        Dictionary<Type, int> colors,
        Dictionary<Type, Type?> parent)
    {
        colors[current] = 1; // Gray - currently being processed

        if (graph.TryGetValue(current, out var dependencies))
        {
            foreach (var dependency in dependencies)
            {
                // Dependencies not yet registered cannot participate in a cycle.
                if (!colors.ContainsKey(dependency))
                {
                    continue;
                }

                if (colors[dependency] == 1)
                {
                    // Found a cycle - dependency is currently being processed
                    parent[dependency] = current;
                    return dependency;
                }

                if (colors[dependency] == 0)
                {
                    parent[dependency] = current;
                    var cycleStart = DetectCycleDfs(dependency, graph, colors, parent);
                    if (cycleStart != null)
                    {
                        return cycleStart;
                    }
                }
            }
        }

        colors[current] = 2; // Black - fully processed
        return null;
    }

    /// <summary>
    /// Builds the path of a detected cycle for error reporting.
    /// </summary>
    private static List<Type> BuildCyclePath(Type cycleStart, Dictionary<Type, Type?> parent)
    {
        var path = new List<Type> { cycleStart };
        var current = parent[cycleStart];

        while (current != null && current != cycleStart)
        {
            path.Add(current);
            current = parent[current];
        }

        path.Add(cycleStart); // Complete the cycle
        path.Reverse();

        return path;
    }
}
