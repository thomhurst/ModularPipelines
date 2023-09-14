using System.Diagnostics;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IModuleDisposeExecutor
{
    [StackTraceHidden]
    Task ExecuteAndDispose(IEnumerable<ModuleBase> modules, Func<Task> executeDelegate);
}