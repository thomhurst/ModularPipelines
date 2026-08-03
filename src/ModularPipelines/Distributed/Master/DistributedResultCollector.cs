using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Master;

internal class DistributedResultCollector(
    IDistributedCoordinator coordinator,
    ModuleResultSerializer serializer,
    ICommandExecutionCounter? commandExecutionCounter = null,
    IOptions<DistributedOptions>? distributedOptions = null)
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly ICommandExecutionCounter? _commandExecutionCounter = commandExecutionCounter;
    private readonly IOptions<DistributedOptions>? _distributedOptions = distributedOptions;

    public async Task<IModuleResult?> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var serialized = await _coordinator.WaitForResultAsync(moduleTypeName, cancellationToken);
        var result = _serializer.Deserialize(serialized);
        if (result is ModuleResult { ModuleType: { } moduleType }
            && serialized.WorkerIndex != _distributedOptions?.Value.InstanceIndex)
        {
            _commandExecutionCounter?.Add(moduleType, serialized.CommandCount);
        }

        return result;
    }
}
