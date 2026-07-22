using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IDependencyChainProvider
{
    IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }

    void Initialize(IReadOnlyList<IModule> modules);
}
