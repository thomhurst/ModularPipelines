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
    /// Gets all dependencies declared on a module type via DependsOn attributes.
    /// This overload only handles DependsOnAttribute, not DependsOnAllModulesInheritingFromAttribute.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetDependencies(Type moduleType)
    {
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.Optional);
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes,
    /// including DependsOnAllModulesInheritingFromAttribute which requires the list of available modules.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetDependencies(
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
    public static IEnumerable<(Type DependencyType, bool Optional)> GetDependencies(
        Type moduleType,
        IEnumerable<Type> availableModuleTypes,
        IDependencyContext? dependencyContext)
    {
        var availableModuleTypesList = availableModuleTypes as IReadOnlyList<Type> ?? availableModuleTypes.ToList();

        // Handle regular DependsOn attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.Optional);
        }

        foreach (var dependency in GetSelectorDependencies(moduleType, availableModuleTypesList, dependencyContext))
        {
            yield return dependency;
        }
    }

    /// <summary>
    /// Gets dependencies selected from the available module set by base-type or metadata predicates.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetSelectorDependencies(
        Type moduleType,
        IReadOnlyList<Type> availableModuleTypes,
        IDependencyContext? dependencyContext)
    {
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
                    yield return (candidateType, false);
                }
            }
        }

        // Handle predicate-based dependencies (DependsOnBaseAttribute derivatives)
        if (dependencyContext != null)
        {
            foreach (var dep in GetPredicateDependencies(moduleType, availableModuleTypes, dependencyContext))
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
    /// <returns>Enumerable of dependency tuples (DependencyType, Optional).</returns>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetPredicateDependencies(
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
    /// Gets dependencies from the module's canonical configuration.
    /// </summary>
    /// <param name="module">The module instance to inspect.</param>
    /// <returns>An enumerable of dependency tuples (DependencyType, Optional).</returns>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetConfiguredDependencies(IModule module)
    {
        foreach (var dep in module.Configuration.Dependencies)
        {
            yield return (dep.ModuleType, dep.IsOptional);
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes,
    /// including both static (attribute-based) and dynamic (runtime-added) dependencies.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetAllDependencies(
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
    /// - Dependencies from the canonical module configuration.
    /// - Dependencies selected by base-type attributes.
    /// - Dynamic dependencies from the registry.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetAllDependencies(
        IModule module,
        IEnumerable<Type> availableModuleTypes,
        IModuleDependencyRegistry? dynamicRegistry = null,
        IDependencyContext? dependencyContext = null)
    {
        var moduleType = module.GetType();

        foreach (var dep in GetConfiguredDependencies(module))
        {
            yield return dep;
        }

        var availableModuleTypesList = availableModuleTypes as IReadOnlyList<Type> ?? availableModuleTypes.ToList();
        foreach (var dep in GetSelectorDependencies(moduleType, availableModuleTypesList, dependencyContext))
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
