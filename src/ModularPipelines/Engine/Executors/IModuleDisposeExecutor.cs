using System.Diagnostics;
using System.Runtime.CompilerServices;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IModuleDisposeExecutor
{
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    Task ExecuteAndDispose(IEnumerable<ModuleBase> modules, Func<Task> executeDelegate);
}