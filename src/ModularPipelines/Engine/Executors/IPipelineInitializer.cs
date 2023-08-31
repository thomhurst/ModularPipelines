using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal interface IPipelineInitializer
{
    Task<OrganizedModules> Initialize();
}