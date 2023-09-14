using System.Diagnostics;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintProgressExecutor
{
    [StackTraceHidden]
    Task ExecuteWithProgress(OrganizedModules organizedModules, Func<Task> executeDelegate);
}