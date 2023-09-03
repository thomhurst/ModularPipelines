using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintProgressExecutor
{
    Task ExecuteWithProgress(OrganizedModules organizedModules, Func<Task> executeDelegate);
}