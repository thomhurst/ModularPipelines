using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IModuleRetriever
{
    Task<OrganizedModules> GetOrganizedModules();
}