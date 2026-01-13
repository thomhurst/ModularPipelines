using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Resolves module dependencies by inspecting DependsOn attributes and programmatic declarations.
/// </summary>
internal static class ModuleDependencyResolver
{
    /// <summary>
    /// Cache for GetDeclaredDependencies method lookups to avoid repeated reflection.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, MethodInfo?> GetDeclaredDependenciesMethodCache = new();

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes.
    /// This overload only handles DependsOnAttribute, not DependsOnAllModulesInheritingFromAttribute.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(Type moduleType)
    {
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.IgnoreIfNotRegistered);
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes,
    /// including DependsOnAllModulesInheritingFromAttribute which requires the list of available modules.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(
        Type moduleType,
        IEnumerable<Type> availableModuleTypes)
    {
        return GetDependencies(moduleType, availableModuleTypes, dependencyContext: null);
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes,
    /// including DependsOnAllModulesInheritingFromAttribute and predicate-based DependsOnBaseAttribute derivatives.
    /// </summary>
    /// <param name="moduleType">The module type to get dependencies for.</param>
    /// <param name="availableModuleTypes">All available module types in the pipeline.</param>
    /// <param name="dependencyContext">Context providing access to module metadata (tags, categories, attributes).
    /// Required for predicate-based dependencies. If null, predicate-based dependencies are skipped.</param>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(
        Type moduleType,
        IEnumerable<Type> availableModuleTypes,
        IDependencyContext? dependencyContext)
    {
        var availableModuleTypesList = availableModuleTypes as IReadOnlyList<Type> ?? availableModuleTypes.ToList();

        // Handle regular DependsOn attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.IgnoreIfNotRegistered);
        }

        // Handle DependsOnAllModulesInheritingFrom attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAllModulesInheritingFromAttribute>())
        {
            foreach (var candidateType in availableModuleTypesList)
            {
                // Skip self
                if (candidateType == moduleType)
                {
                    continue;
                }

                // Check if candidate inherits from the specified base type
                if (candidateType.IsOrInheritsFrom(attribute.Type))
                {
                    yield return (candidateType, false);
                }
            }
        }

        // Handle predicate-based dependencies (DependsOnBaseAttribute derivatives)
        if (dependencyContext != null)
        {
            foreach (var dep in GetPredicateDependencies(moduleType, availableModuleTypesList, dependencyContext))
            {
                yield return dep;
            }
        }
    }

    /// <summary>
    /// Gets dependencies resolved via predicate-based attributes (DependsOnBaseAttribute derivatives).
    /// </summary>
    /// <param name="moduleType">The module type to get predicate dependencies for.</param>
    /// <param name="availableModuleTypes">All available module types to evaluate against predicates.</param>
    /// <param name="dependencyContext">Context providing access to module metadata.</param>
    /// <returns>Enumerable of dependency tuples (DependencyType, IgnoreIfNotRegistered).</returns>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetPredicateDependencies(
        Type moduleType,
        IReadOnlyList<Type> availableModuleTypes,
        IDependencyContext dependencyContext)
    {
        var predicateAttributes = moduleType
            .GetCustomAttributesIncludingBaseInterfaces<DependsOnBaseAttribute>()
            .ToList();

        if (predicateAttributes.Count == 0)
        {
            yield break;
        }

        foreach (var candidateType in availableModuleTypes)
        {
            // Skip self
            if (candidateType == moduleType)
            {
                continue;
            }

            foreach (var attr in predicateAttributes)
            {
                if (attr.ShouldDependOn(candidateType, dependencyContext))
                {
                    yield return (candidateType, false);
                    break; // Only add once even if multiple attributes match
                }
            }
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module via DependsOn attributes.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(IModule module)
    {
        return GetDependencies(module.GetType());
    }

    /// <summary>
    /// Gets programmatic dependencies declared via DeclareDependencies method on a module instance.
    /// </summary>
    /// <param name="module">The module instance to get programmatic dependencies from.</param>
    /// <returns>An enumerable of dependency tuples (DependencyType, IgnoreIfNotRegistered).</returns>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetProgrammaticDependencies(IModule module)
    {
        // Use cached reflection lookup for GetDeclaredDependencies method
        var moduleType = module.GetType();
        var method = GetDeclaredDependenciesMethodCache.GetOrAdd(moduleType, type =>
            type.GetMethod("GetDeclaredDependencies",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

        if (method == null)
        {
            yield break;
        }

        var dependencies = method.Invoke(module, null) as IReadOnlyList<DeclaredDependency>;

        if (dependencies == null)
        {
            yield break;
        }

        foreach (var dep in dependencies)
        {
            yield return (dep.ModuleType, dep.IgnoreIfNotRegistered);
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes,
    /// including both static (attribute-based) and dynamic (runtime-added) dependencies.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetAllDependencies(
        Type moduleType,
        IEnumerable<Type> availableModuleTypes,
        IModuleDependencyRegistry? dynamicRegistry = null)
    {
        // Static dependencies from attributes
        foreach (var dep in GetDependencies(moduleType, availableModuleTypes))
        {
            yield return dep;
        }

        // Dynamic dependencies from registration
        if (dynamicRegistry != null)
        {
            foreach (var dynamicDep in dynamicRegistry.GetDynamicDependencies(moduleType))
            {
                yield return (dynamicDep, false);
            }
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module instance, including:
    /// - Static dependencies from DependsOn attributes
    /// - Programmatic dependencies from DeclareDependencies method
    /// - Dynamic dependencies from the registry
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetAllDependencies(
        IModule module,
        IEnumerable<Type> availableModuleTypes,
        IModuleDependencyRegistry? dynamicRegistry = null)
    {
        var moduleType = module.GetType();

        // Static dependencies from attributes
        foreach (var dep in GetDependencies(moduleType, availableModuleTypes))
        {
            yield return dep;
        }

        // Programmatic dependencies from DeclareDependencies method
        foreach (var dep in GetProgrammaticDependencies(module))
        {
            yield return dep;
        }

        // Dynamic dependencies from registration
        if (dynamicRegistry != null)
        {
            foreach (var dynamicDep in dynamicRegistry.GetDynamicDependencies(moduleType))
            {
                yield return (dynamicDep, false);
            }
        }
    }
}
