using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal static class ModuleResultRegistryExtensions
{
    public static IReadOnlyList<IModuleResult> GetCompletedResults(
        this IModuleResultRegistry resultRegistry,
        IEnumerable<IModule> modules)
    {
        ArgumentNullException.ThrowIfNull(resultRegistry);
        ArgumentNullException.ThrowIfNull(modules);

        return modules
            .Select(module => resultRegistry.GetResult(module.GetType()))
            .OfType<IModuleResult>()
            .ToArray();
    }
}
