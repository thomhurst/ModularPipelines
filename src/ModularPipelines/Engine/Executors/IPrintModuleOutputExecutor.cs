using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IPrintModuleOutputExecutor
{
    Task<IReadOnlyList<ModuleBase>> ExecuteAndPrintModuleOutput(Func<Task<IReadOnlyList<ModuleBase>>> executeDelegate);
}