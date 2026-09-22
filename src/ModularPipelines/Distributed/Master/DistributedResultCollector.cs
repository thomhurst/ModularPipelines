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

    public async Task<IModuleResult?> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var serialized = await _coordinator.WaitForResultAsync(moduleId, cancellationToken)
            .ConfigureAwait(false);
        var receivedAt = DateTimeOffset.UtcNow;
        var result = _serializer.Deserialize(serialized);
        if (result is ModuleResult { ModuleType: { } moduleType })
        {
            _telemetryTracker?.RecordResult(serialized, receivedAt, ModuleTypeIdentifier.Get(moduleType));
            if (serialized.WorkerIndex != _distributedOptions?.Value.InstanceIndex)
            {
                _commandExecutionCounter?.AddRemote(
                    moduleType,
                    serialized.WorkerIndex,
                    serialized.CommandCount);
            }
        }

        return result;
    }
}
