using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;

namespace ModularPipelines.Engine;

/// <summary>
/// Validates the module dependency graph at registration time to detect circular dependencies.
/// Uses depth-first search (DFS) to detect cycles before modules are instantiated.
/// </summary>
internal static class DependencyGraphValidator
{
    /// <summary>
    /// Validates that the given module types do not form any circular dependencies.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Limitation:</b> This method only validates attribute-based dependencies declared via
    /// <see cref="DependsOnAttribute"/> and <see cref="DependsOnAllModulesInheritingFromAttribute"/>.
    /// Dependencies declared through <c>Module.Configure()</c> cannot be detected at registration
    /// time and are NOT validated by this method.
    /// </para>
    /// <para>
    /// Circular dependencies involving only fluent configuration will still fail at runtime
    /// during module execution.
    /// </para>
    /// </remarks>
    /// <param name="moduleTypes">The collection of module types to validate.</param>
    /// <exception cref="CircularDependencyException">Thrown when a circular dependency is detected.</exception>
    public static void ValidateNoCycles(IEnumerable<Type> moduleTypes)
    {
        var moduleTypeSet = moduleTypes.ToHashSet();

        // Build adjacency list: module type -> its dependencies (types it depends on)
        var adjacencyList = BuildAdjacencyList(moduleTypeSet);

        var cycle = DependencyCycleDetector.FindCycle(adjacencyList);
        if (cycle is not null)
        {
            throw CircularDependencyException.CreateWithCyclePath(cycle);
        }
    }

    /// <summary>
    /// Builds an adjacency list representing module dependencies.
    /// </summary>
    private static Dictionary<Type, List<Type>> BuildAdjacencyList(HashSet<Type> moduleTypes)
    {
        var adjacencyList = new Dictionary<Type, List<Type>>();

        foreach (var moduleType in moduleTypes)
        {
            var dependencies = GetDependencyTypes(moduleType, moduleTypes);
            adjacencyList[moduleType] = [.. dependencies];
        }

        return adjacencyList;
    }

    /// <summary>
    /// Gets the dependency types for a module by inspecting its DependsOn attributes.
    /// </summary>
    private static IEnumerable<Type> GetDependencyTypes(Type moduleType, HashSet<Type> availableModuleTypes)
    {
        // Get direct DependsOn attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            // Only include if this dependency type is actually being registered
            // Also handle Optional - if the dependency is not registered and
            // Optional is true, we skip it for cycle detection
            if (availableModuleTypes.Contains(attribute.Type))
            {
                yield return attribute.Type;
            }
            else if (!attribute.Optional)
            {
                // If the dependency is not registered and Optional is false,
                // we still yield it so the runtime can fail appropriately later.
                // For cycle detection, we only care about registered modules.
            }
        }

        // Get DependsOnAllModulesInheritingFrom attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAllModulesInheritingFromAttribute>())
        {
            foreach (var candidateType in availableModuleTypes)
            {
                // Skip self
                if (candidateType == moduleType)
                {
                    continue;
                }

                // Check if candidate inherits from the specified base type
                if (candidateType.IsOrInheritsFrom(attribute.Type))
                {
                    yield return candidateType;
                }
            }
        }
    }
}
