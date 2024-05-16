using ModularPipelines.Enums;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IWaitHandler
{
    Task WaitForModuleDependencies();
}