using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record OrganizedModules(IReadOnlyList<RunnableModule> RunnableModules, IReadOnlyList<ModuleBase> IgnoredModules)
{
    public IReadOnlyList<ModuleBase> AllModules { get; } =
    [
        ..RunnableModules.Select(x => x.Module),
        ..IgnoredModules
    ];
}