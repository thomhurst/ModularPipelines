using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Modules;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates module dependencies including circular dependencies, missing dependencies, and self-references.
/// </summary>
internal class DependencyValidator : IDependencyValidator
{
    /// <inheritdoc />
    public int Order => 200;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services)
    {
        var modules = services.GetServices<IModule>();
        var moduleTypes = modules.Select(m => m.GetType());
        return ValidateDependencies(moduleTypes);
    }

    /// <inheritdoc />
    public ValidationResult ValidateDependencies(IEnumerable<Type> moduleTypes)
    {
        var result = new ValidationResult();
        var types = moduleTypes.ToHashSet();

        if (types.Count == 0)
        {
            return result;
        }

        ValidateSelfReferences(types, result);
        ValidateMissingDependencies(types, result);
        ValidateCircularDependencies(types, result);

        return result;
    }

    /// <summary>
    /// Validates that no module references itself.
    /// </summary>
    private static void ValidateSelfReferences(HashSet<Type> moduleTypes, ValidationResult result)
    {
        foreach (var moduleType in moduleTypes)
        {
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType, moduleTypes);

            foreach (var (dependencyType, _) in dependencies)
            {
                if (dependencyType == moduleType)
                {
                    result.AddError(new ValidationError(
                        ValidationErrorCategory.Dependency,
                        $"Module '{moduleType.Name}' cannot reference itself. A module cannot depend on its own result.",
                        moduleType));
                }
            }
        }
    }

    /// <summary>
    /// Validates that all dependencies are registered.
    /// </summary>
    private static void ValidateMissingDependencies(HashSet<Type> moduleTypes, ValidationResult result)
    {
        foreach (var moduleType in moduleTypes)
        {
            // Use the overload without availableModuleTypes to get ALL declared dependencies
            var dependencies = ModuleDependencyResolver.GetDependencies(moduleType);

            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                if (ignoreIfNotRegistered)
                {
                    continue;
                }

                if (!moduleTypes.Contains(dependencyType))
                {
                    result.AddError(new ValidationError(
                        ValidationErrorCategory.Dependency,
                        $"Module '{moduleType.Name}' depends on '{dependencyType.Name}', " +
                        $"but '{dependencyType.Name}' is not registered. " +
                        "Either register the dependency module or set IgnoreIfNotRegistered = true on the [DependsOn] attribute.",
                        moduleType));
                }
            }
        }
    }

    /// <summary>
    /// Validates that there are no circular dependencies between modules.
    /// </summary>
    private static void ValidateCircularDependencies(HashSet<Type> moduleTypes, ValidationResult result)
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

                    result.AddError(new ValidationError(
                        ValidationErrorCategory.Dependency,
                        $"Circular dependency detected: {cycleDescription}",
                        cycleStart));
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
