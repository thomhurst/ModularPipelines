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
    /// Validates all registered module dependencies.
    /// </summary>
    /// <param name="registeredModuleTypes">The types of all registered modules.</param>
    /// <exception cref="InvalidModuleConfigurationException">
    /// Thrown when validation fails due to missing dependencies, circular dependencies, or self-references.
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
                    throw new InvalidModuleConfigurationException(
                        $"Module '{moduleType.Name}' cannot depend on itself. " +
                        "Remove the self-referencing [DependsOn] attribute.");
                }
            }
        }
    }

    /// <summary>
    /// Validates that all dependencies are registered.
    /// </summary>
    private static void ValidateMissingDependencies(HashSet<Type> moduleTypes)
    {
        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes);

            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                if (ignoreIfNotRegistered)
                {
                    continue;
                }

                if (!moduleTypes.Contains(dependencyType))
                {
                    throw new InvalidModuleConfigurationException(
                        $"Module '{moduleType.Name}' depends on '{dependencyType.Name}', " +
                        $"but '{dependencyType.Name}' is not registered. " +
                        "Either register the dependency module or set IgnoreIfNotRegistered = true on the [DependsOn] attribute.");
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
        var dependencyGraph = new Dictionary<Type, List<Type>>();

        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes)
                .Where(d => moduleTypes.Contains(d.DependencyType))
                .Select(d => d.DependencyType)
                .ToList();

            dependencyGraph[moduleType] = dependencies;
        }

        // Detect cycles using DFS with coloring
        // White (0) = not visited, Gray (1) = in current path, Black (2) = fully processed
        var colors = new Dictionary<Type, int>();
        var parent = new Dictionary<Type, Type?>();

        foreach (var moduleType in moduleTypes)
        {
            colors[moduleType] = 0;
            parent[moduleType] = null;
        }

        foreach (var moduleType in moduleTypes)
        {
            if (colors[moduleType] == 0)
            {
                var cycleStart = DetectCycleDfs(moduleType, dependencyGraph, colors, parent);
                if (cycleStart != null)
                {
                    var cycle = BuildCyclePath(cycleStart, parent);
                    var cycleDescription = string.Join(" -> ", cycle.Select(t => t.Name));

                    throw new InvalidModuleConfigurationException(
                        $"Circular dependency detected: {cycleDescription}. " +
                        "Modules cannot have circular dependencies.");
                }
            }
        }
    }

    /// <summary>
    /// Performs DFS to detect cycles, returning the start of a cycle if found.
    /// </summary>
    private static Type? DetectCycleDfs(
        Type current,
        Dictionary<Type, List<Type>> graph,
        Dictionary<Type, int> colors,
        Dictionary<Type, Type?> parent)
    {
        colors[current] = 1; // Gray - currently being processed

        if (graph.TryGetValue(current, out var dependencies))
        {
            foreach (var dependency in dependencies)
            {
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
