using System.Runtime.CompilerServices;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate);
}