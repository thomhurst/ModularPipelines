using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Master;

internal class DistributedResultCollector(
    IDistributedMasterCoordinator coordinator,
    ModuleResultSerializer serializer,
    ICommandExecutionCounter? commandExecutionCounter = null,
    IOptions<DistributedOptions>? distributedOptions = null,
    DistributedTelemetryTracker? telemetryTracker = null)
{
    private readonly IDistributedMasterCoordinator _coordinator = coordinator;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly ICommandExecutionCounter? _commandExecutionCounter = commandExecutionCounter;
    private readonly IOptions<DistributedOptions>? _distributedOptions = distributedOptions;
    private readonly DistributedTelemetryTracker? _telemetryTracker = telemetryTracker;

    public async Task<IModuleResult?> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var serialized = await _coordinator.WaitForResultAsync(moduleTypeName, cancellationToken)
            .ConfigureAwait(false);
        _telemetryTracker?.RecordResult(serialized, DateTimeOffset.UtcNow);
        var result = _serializer.Deserialize(serialized);
        if (result is ModuleResult { ModuleType: { } moduleType }
            && serialized.WorkerIndex != _distributedOptions?.Value.InstanceIndex)
        {
            _commandExecutionCounter?.AddRemote(
                moduleType,
                serialized.WorkerIndex,
                serialized.CommandCount);
        }

        return result;
    }
}
