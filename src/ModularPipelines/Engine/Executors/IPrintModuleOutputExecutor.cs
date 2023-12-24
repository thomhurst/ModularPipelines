using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate);
}