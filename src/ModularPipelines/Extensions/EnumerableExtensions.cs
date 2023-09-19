using ModularPipelines.Modules;

namespace ModularPipelines.Extensions;

public static class EnumerableExtensions
{
    public static T GetModule<T>(this IEnumerable<ModuleBase> modules) where T : ModuleBase => modules.OfType<T>().Single();
}