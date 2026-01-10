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
    /// Dependencies declared programmatically via <c>Module.DeclareDependencies()</c> at runtime
    /// cannot be detected at registration time and are NOT validated by this method.
    /// </para>
    /// <para>
    /// Circular dependencies involving only programmatic declarations will still fail at runtime
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

        // Track visit states for cycle detection
        // 0 = not visited, 1 = in current DFS path (gray), 2 = fully processed (black)
        var visitState = new Dictionary<Type, int>();
        foreach (var moduleType in moduleTypeSet)
        {
            visitState[moduleType] = 0;
        }

        // Track the current path for error reporting
        var currentPath = new List<Type>();

        // Run DFS from each unvisited node
        foreach (var moduleType in moduleTypeSet)
        {
            if (visitState[moduleType] == 0)
            {
                if (DetectCycleDfs(moduleType, adjacencyList, visitState, currentPath, moduleTypeSet))
                {
                    // Cycle detected - currentPath contains the cycle
                    throw CircularDependencyException.CreateWithCyclePath(currentPath.ToList());
                }
            }
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
            adjacencyList[moduleType] = dependencies.ToList();
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
            // Also handle IgnoreIfNotRegistered - if the dependency is not registered and
            // IgnoreIfNotRegistered is true, we skip it for cycle detection
            if (availableModuleTypes.Contains(attribute.Type))
            {
                yield return attribute.Type;
            }
            else if (!attribute.IgnoreIfNotRegistered)
            {
                // If the dependency is not registered and IgnoreIfNotRegistered is false,
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

    /// <summary>
    /// Performs depth-first search to detect cycles.
    /// </summary>
    /// <returns>True if a cycle is detected, false otherwise.</returns>
    private static bool DetectCycleDfs(
        Type current,
        Dictionary<Type, List<Type>> adjacencyList,
        Dictionary<Type, int> visitState,
        List<Type> currentPath,
        HashSet<Type> moduleTypeSet)
    {
        // Mark as in-progress (gray)
        visitState[current] = 1;
        currentPath.Add(current);

        // Check all dependencies
        if (adjacencyList.TryGetValue(current, out var dependencies))
        {
            foreach (var dependency in dependencies)
            {
                // Skip dependencies that aren't in our module set (they're not being registered)
                if (!moduleTypeSet.Contains(dependency))
                {
                    continue;
                }

                if (visitState.TryGetValue(dependency, out var state))
                {
                    if (state == 1)
                    {
                        // Found a back edge - cycle detected!
                        // Add the dependency again to show the complete cycle
                        currentPath.Add(dependency);

                        // Trim the path to show only the cycle
                        var cycleStartIndex = currentPath.IndexOf(dependency);
                        if (cycleStartIndex >= 0 && cycleStartIndex < currentPath.Count - 1)
                        {
                            currentPath.RemoveRange(0, cycleStartIndex);
                        }

                        return true;
                    }

                    if (state == 0)
                    {
                        // Not visited - recurse
                        if (DetectCycleDfs(dependency, adjacencyList, visitState, currentPath, moduleTypeSet))
                        {
                            return true;
                        }
                    }
                    // If state == 2, already fully processed, no cycle through this node
                }
            }
        }

        // Mark as fully processed (black)
        visitState[current] = 2;
        currentPath.RemoveAt(currentPath.Count - 1);

        return false;
    }
}
