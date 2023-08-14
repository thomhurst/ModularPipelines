namespace ModularPipelines.Models;

internal class SkippedModuleResult<T> : ModuleResult<T>
{
    public SkippedModuleResult() : base(default(T?))
    {
        ModuleResultType = ModuleResultType.Skipped;
    }
}
