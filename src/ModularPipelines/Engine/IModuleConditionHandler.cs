using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleConditionHandler
{
    Task<bool> ShouldIgnore(ModuleBase module);
}
