namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate);
}