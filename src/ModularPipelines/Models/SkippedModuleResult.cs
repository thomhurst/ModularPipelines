using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal class SkippedModuleResult<T> : ModuleResult<T>
{
    internal SkippedModuleResult(ModuleBase module) : base(default(T?), module)
    {
        ModuleResultType = ModuleResultType.Skipped;
    }
}
