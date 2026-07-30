using ModularPipelines.Engine;
using ModularPipelines.Modules;

namespace ModularPipelines.Caching;

internal interface IModuleCacheResultRepository : IModuleResultRepository
{
    void DiscardFingerprint(IModule module);
}
