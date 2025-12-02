using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleConditionHandler
{
    Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module);
}