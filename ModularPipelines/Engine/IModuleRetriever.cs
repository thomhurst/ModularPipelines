using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IModuleRetriever
{
    OrganizedModules GetOrganizedModules();
}