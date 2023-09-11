using System.Runtime.CompilerServices;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IModuleDisposeExecutor
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Task ExecuteAndDispose(IEnumerable<ModuleBase> modules, Func<Task> executeDelegate);
}