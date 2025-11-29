using System.Reflection;
using ModularPipelines.Attributes;
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
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(Type moduleType)
    {
        foreach (var attribute in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return (attribute.Type, attribute.IgnoreIfNotRegistered);
        }
    }

    /// <summary>
    /// Gets all dependencies declared on a module via DependsOn attributes.
    /// </summary>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(IModule module)
    {
        return GetDependencies(module.GetType());
    }
}
