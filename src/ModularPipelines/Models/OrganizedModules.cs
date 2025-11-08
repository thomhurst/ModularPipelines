using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record OrganizedModules(IReadOnlyList<RunnableModule> RunnableModules, IReadOnlyList<IModule> IgnoredModules)
{
    public IReadOnlyList<IModule> AllModules { get; } = RunnableModules.Select(x => x.Module).Concat(IgnoredModules).ToList();
}