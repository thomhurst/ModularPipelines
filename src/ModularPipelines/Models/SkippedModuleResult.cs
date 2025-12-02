using ModularPipelines.Engine;

namespace ModularPipelines.Models;

internal class SkippedModuleResult<T> : ModuleResult<T>
{
    internal SkippedModuleResult(ModuleExecutionContext<T> executionContext, SkipDecision skipDecision)
        : base(default(T?), executionContext)
    {
        SkipDecision = skipDecision;
    }
}