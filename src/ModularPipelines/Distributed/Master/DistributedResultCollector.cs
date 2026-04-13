using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Master;

internal class DistributedResultCollector(
    IDistributedCoordinator coordinator,
    ModuleResultSerializer serializer)
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleResultSerializer _serializer = serializer;

    public async Task<IModuleResult?> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var serialized = await _coordinator.WaitForResultAsync(moduleTypeName, cancellationToken);
        return _serializer.Deserialize(serialized);
    }
}
