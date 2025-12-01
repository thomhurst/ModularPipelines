using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record IgnoredModule(IModule Module, SkipDecision SkipDecision);

internal record OrganizedModules(IReadOnlyList<RunnableModule> RunnableModules, IReadOnlyList<IgnoredModule> IgnoredModules)
{
    public IReadOnlyList<IModule> AllModules { get; } = RunnableModules.Select(x => x.Module).Concat(IgnoredModules.Select(x => x.Module)).ToList();
}