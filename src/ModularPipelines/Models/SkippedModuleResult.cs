using ModularPipelines.Engine;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal class SkippedModuleResult<T> : ModuleResult<T>
{
    internal SkippedModuleResult(ModuleBase module, SkipDecision skipDecision) : base(default(T?), module)
    {
        SkipDecision = skipDecision;
    }

    internal SkippedModuleResult(ModuleExecutionContext<T> executionContext, SkipDecision skipDecision)
        : base(default(T?), executionContext)
    {
        SkipDecision = skipDecision;
    }
}