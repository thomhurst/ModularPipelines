using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintProgressExecutor
{
    Task<IReadOnlyList<ModuleBase>> ExecuteWithProgress(OrganizedModules organizedModules, Func<Task<IReadOnlyList<ModuleBase>>> executeDelegate);
}