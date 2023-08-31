using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintProgressExecutor
{
    Task ExecuteWithProgress(OrganizedModules organizedModules, Func<Task> executeDelegate);
}