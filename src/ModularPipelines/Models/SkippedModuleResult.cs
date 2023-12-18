using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal class SkippedModuleResult<T> : ModuleResult<T>
{
    internal SkippedModuleResult(ModuleBase module, SkipDecision skipDecision) : base(default(T?), module)
    {
        SkipDecision = skipDecision;
    }
}