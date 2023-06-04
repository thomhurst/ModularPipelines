using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record OrganizedModules(IReadOnlyList<ModuleBase> RunnableModules, IReadOnlyList<ModuleBase> IgnoredModules)
{
    public IReadOnlyList<ModuleBase> AllModules { get; } = RunnableModules.Concat(IgnoredModules).ToList();
};