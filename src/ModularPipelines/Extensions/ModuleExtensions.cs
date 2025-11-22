using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extension methods for IModule.
/// </summary>
public static class ModuleExtensions
{
    /// <summary>
    /// Gets all module dependencies declared via [DependsOn] attributes.
    /// </summary>
    /// <param name="module">The module to get dependencies for.</param>
    /// <returns>A collection of tuples containing the dependency type and whether it should be ignored if not registered.</returns>
    public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetModuleDependencies(this IModule module)
    {
        var moduleType = module.GetType();
        var dependsOnAttributes = moduleType.GetCustomAttributes<DependsOnAttribute>(inherit: true);

        foreach (var attribute in dependsOnAttributes)
        {
            yield return (attribute.Type, attribute.IgnoreIfNotRegistered);
        }
    }

    /// <summary>
    /// Gets the module run type from the [AlwaysRun] attribute.
    /// </summary>
    /// <param name="module">The module to check.</param>
    /// <returns>AlwaysRun if the attribute is present, otherwise OnSuccessfulDependencies.</returns>
    public static Models.ModuleRunType GetModuleRunType(this IModule module)
    {
        var moduleType = module.GetType();
        var alwaysRunAttribute = moduleType.GetCustomAttribute<AlwaysRunAttribute>(inherit: true);

        return alwaysRunAttribute != null
            ? Models.ModuleRunType.AlwaysRun
            : Models.ModuleRunType.OnSuccessfulDependencies;
    }
}
