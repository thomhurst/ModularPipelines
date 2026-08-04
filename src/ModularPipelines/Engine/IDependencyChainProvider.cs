using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IDependencyChainProvider
{
    bool IsInitialized { get; }

    IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }

    void Initialize(IReadOnlyList<IModule> modules);
}
