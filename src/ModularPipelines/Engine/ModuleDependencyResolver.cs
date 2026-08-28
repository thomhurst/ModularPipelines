using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

using ModularPipelines.Generated;

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
        foreach (var dependency in GetDeclaredDependencies(moduleType))
        {
            yield return dependency;
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
        foreach (var dependency in GetDeclaredDependencies(moduleType))
        {
            yield return dependency;
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
        IDependencyContext? dependencyContext,
        bool planningSafeOnly = false)
    {
        planningSafeOnly |= dependencyContext is ModuleMetadataRegistry { PlanningSafeOnly: true };

        foreach (var attribute in GetInheritanceSelectorAttributes(moduleType, planningSafeOnly))
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
            foreach (var dep in GetPredicateDependencies(
                         moduleType,
                         availableModuleTypes,
                         dependencyContext,
                         planningSafeOnly))
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
    /// <param name="planningSafeOnly">Whether to evaluate only predicates explicitly safe for planning.</param>
    /// <returns>Enumerable of dependency tuples (DependencyType, Optional).</returns>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetPredicateDependencies(
        Type moduleType,
        IReadOnlyList<Type> availableModuleTypes,
        IDependencyContext dependencyContext,
        bool planningSafeOnly = false)
    {
        var predicateAttributes = planningSafeOnly
            ? moduleType
                .GetCustomAttributesIncludingBaseInterfaces<PlanningSafeDependsOnBaseAttribute>()
                .Cast<DependsOnBaseAttribute>()
                .ToList()
            : moduleType
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
    /// - Dependencies declared by <see cref="DependsOnAttribute"/>.
    /// - Dependencies from the canonical module configuration.
    /// - Dependencies selected by base-type attributes.
    /// - Dynamic dependencies from the registry.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetAllDependencies(
        IModule module,
        IEnumerable<Type> availableModuleTypes,
        IModuleDependencyRegistry? dynamicRegistry = null,
        IDependencyContext? dependencyContext = null,
        bool planningSafeOnly = false)
    {
        var moduleType = module.GetType();

        var declaredDependencies = GetDependencies(moduleType)
            .Concat(GetConfiguredDependencies(module))
            .GroupBy(dependency => dependency.DependencyType)
            .Select(group => (
                DependencyType: group.Key,
                Optional: group.All(dependency => dependency.Optional)));

        foreach (var dep in declaredDependencies)
        {
            yield return dep;
        }

        var availableModuleTypesList = availableModuleTypes as IReadOnlyList<Type> ?? availableModuleTypes.ToList();
        foreach (var dep in GetSelectorDependencies(
                     moduleType,
                     availableModuleTypesList,
                     dependencyContext,
                     planningSafeOnly))
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

    private static IEnumerable<(Type DependencyType, bool Optional)> GetDeclaredDependencies(Type moduleType)
    {
        if (GeneratedModuleMetadata.TryGetDependencies(moduleType, out var generatedDependencies))
        {
            return generatedDependencies.Select(static dependency => (
                dependency.DependencyType,
                dependency.Optional));
        }

        return moduleType
            .GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>()
            .Select(static attribute => (attribute.Type, attribute.Optional));
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "This is the documented reflection fallback for dynamically supplied module types.")]
    private static IEnumerable<DependsOnAllModulesInheritingFromAttribute> GetInheritanceSelectorAttributes(
        Type moduleType,
        bool planningSafeOnly)
    {
        if (!planningSafeOnly)
        {
            return moduleType
                .GetCustomAttributesIncludingBaseInterfaces<DependsOnAllModulesInheritingFromAttribute>();
        }

        var selectorType = typeof(DependsOnAllModulesInheritingFromAttribute);
        var attributeData = CustomAttributeMetadata.GetApplicable(
                moduleType,
                type => type.IsAssignableTo(selectorType))
            .Concat(moduleType.GetInterfaces()
                .SelectMany(static type => type.CustomAttributes)
                .Where(data => data.AttributeType
                    .IsAssignableTo(typeof(DependsOnAllModulesInheritingFromAttribute))));

        return attributeData
            .Where(data => data.AttributeType.Assembly == selectorType.Assembly
                || data.AttributeType.IsAssignableTo(typeof(IPlanningSafeDependencySelector)))
            .Select(CustomAttributeMetadata.Create<DependsOnAllModulesInheritingFromAttribute>)
            .ToArray();
    }
}
