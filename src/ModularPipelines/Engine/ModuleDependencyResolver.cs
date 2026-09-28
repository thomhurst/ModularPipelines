using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Generated;
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
    public static IEnumerable<(Type DependencyType, bool Optional)> GetDependencies([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type moduleType)
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
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type moduleType,
        IEnumerable<Type> availableModuleTypes)
    {
        return GetDependencies(moduleType, availableModuleTypes, dependencyContext: null);
    }

    /// <summary>
    /// Gets all dependencies declared on a module type via DependsOn attributes and
    /// <see cref="DependsOnBaseAttribute"/> selectors.
    /// </summary>
    /// <param name="moduleType">The module type to get dependencies for.</param>
    /// <param name="availableModuleTypes">All available module types in the pipeline.</param>
    /// <param name="dependencyContext">Context providing access to module metadata (tags, categories, attributes).
    /// Required for metadata-based selectors. If null, only <see cref="DependsOnAllModulesInheritingFromAttribute"/> is evaluated.</param>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetDependencies(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type moduleType,
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
    /// Gets dependencies selected from the available module set by <see cref="DependsOnBaseAttribute"/>
    /// predicates, such as <see cref="DependsOnAllModulesInheritingFromAttribute"/>.
    /// </summary>
    /// <param name="moduleType">The module type to get selector dependencies for.</param>
    /// <param name="availableModuleTypes">All available module types to evaluate against predicates.</param>
    /// <param name="dependencyContext">
    /// Context providing access to module metadata. When null, only
    /// <see cref="DependsOnAllModulesInheritingFromAttribute"/>, which needs no metadata, is evaluated.
    /// </param>
    /// <param name="planningSafeOnly">Whether to construct and evaluate only <see cref="IPlanningSafe"/> predicates.</param>
    /// <returns>Enumerable of dependency tuples (DependencyType, Optional).</returns>
    public static IEnumerable<(Type DependencyType, bool Optional)> GetSelectorDependencies(
        Type moduleType,
        IReadOnlyList<Type> availableModuleTypes,
        IDependencyContext? dependencyContext,
        bool planningSafeOnly = false)
    {
        planningSafeOnly |= dependencyContext is ModuleMetadataRegistry { PlanningSafeOnly: true };

        // Without metadata only inheritance selectors can be evaluated, so construct nothing else.
        var selectors = dependencyContext is null
            ? GetInheritanceSelectorAttributes(moduleType)
            : GetSelectorAttributes(moduleType, planningSafeOnly);

        if (selectors.Count == 0)
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

            if (selectors.Any(selector => selector is IInheritanceDependencySelector inheritanceSelector
                    ? candidateType.IsOrInheritsFrom(inheritanceSelector.Type)
                    : selector.ShouldDependOn(candidateType, dependencyContext!)))
            {
                yield return (candidateType, false);
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
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type moduleType,
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

    private static IEnumerable<(Type DependencyType, bool Optional)> GetDeclaredDependencies([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type moduleType)
    {
        if (GeneratedModuleMetadata.TryGetDependencies(moduleType, out var generatedDependencies))
        {
            return generatedDependencies.Select(static dependency => (
                dependency.DependencyType,
                dependency.Optional));
        }

        // Filter by the dependency interface before construction so unrelated attributes are never constructed.
        return moduleType.GetInterfaces()
            .SelectMany(static type => type.GetCustomAttributes(typeof(IModuleDependencyAttribute), inherit: true))
            .Concat(moduleType.GetCustomAttributes(typeof(IModuleDependencyAttribute), inherit: true))
            .Cast<IModuleDependencyAttribute>()
            .Select(static attribute => (attribute.Type, attribute.Optional));
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "This is the documented reflection fallback for dynamically supplied module types.")]
    private static IReadOnlyList<DependsOnBaseAttribute> GetSelectorAttributes(
        Type moduleType,
        bool planningSafeOnly)
    {
        if (!planningSafeOnly)
        {
            return moduleType
                .GetCustomAttributesIncludingBaseInterfaces<DependsOnBaseAttribute>()
                .ToArray();
        }

        // During planning, construct only selectors that are marked planning-safe.
        static bool IsPlanningSafeSelector(Type type) =>
            type.IsAssignableTo(typeof(DependsOnBaseAttribute))
            && type.IsAssignableTo(typeof(IPlanningSafe));

        return CreateSelectors(moduleType, IsPlanningSafeSelector);
    }

    /// <summary>
    /// Constructs only the selectors that need no module metadata, so selectors that are evaluated at runtime
    /// are not constructed before the metadata registry exists.
    /// </summary>
    private static IReadOnlyList<DependsOnBaseAttribute> GetInheritanceSelectorAttributes(Type moduleType) =>
        CreateSelectors(
            moduleType,
            static type => type.IsAssignableTo(typeof(DependsOnBaseAttribute))
                           && type.IsAssignableTo(typeof(IInheritanceDependencySelector)));

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "This is the documented reflection fallback for dynamically supplied module types.")]
    private static IReadOnlyList<DependsOnBaseAttribute> CreateSelectors(Type moduleType, Func<Type, bool> predicate) =>
        moduleType.GetInterfaces()
            .SelectMany(static type => type.CustomAttributes)
            .Where(data => predicate(data.AttributeType))
            .Concat(CustomAttributeMetadata.GetApplicable(moduleType, predicate))
            .Select(CustomAttributeMetadata.Create<DependsOnBaseAttribute>)
            .ToArray();
}
