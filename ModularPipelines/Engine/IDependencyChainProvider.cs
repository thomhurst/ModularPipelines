using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IDependencyChainProvider
{
    IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }
}
