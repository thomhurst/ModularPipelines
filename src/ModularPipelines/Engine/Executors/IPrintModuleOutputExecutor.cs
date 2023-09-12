using System.Diagnostics;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    [StackTraceHidden]
    Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate);
}