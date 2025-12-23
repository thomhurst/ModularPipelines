using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Resolves module dependencies by inspecting DependsOn attributes.
/// </summary>
internal static class ModuleDependencyResolver
{
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
        // Handle regular DependsOn attributes
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.IgnoreIfNotRegistered);
        }

        // Handle DependsOnAllModulesInheritingFrom attributes
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
    }

    /// <summary>
    /// Gets all dependencies declared on a module via DependsOn attributes.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(IModule module)
    {
        return GetDependencies(module.GetType());
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
}
