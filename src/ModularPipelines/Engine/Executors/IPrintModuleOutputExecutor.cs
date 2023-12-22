using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate);
}