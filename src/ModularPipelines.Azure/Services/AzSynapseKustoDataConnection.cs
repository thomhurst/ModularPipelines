using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto")]
public class AzSynapseKustoDataConnection
{
    public AzSynapseKustoDataConnection(
        AzSynapseKustoDataConnectionEventGrid eventGrid,
        AzSynapseKustoDataConnectionEventHub eventHub,
        AzSynapseKustoDataConnectionIotHub iotHub,
        ICommand internalCommand
    )
    {
        EventGrid = eventGrid;
        EventHub = eventHub;
        IotHub = iotHub;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSynapseKustoDataConnectionEventGrid EventGrid { get; }

    public AzSynapseKustoDataConnectionEventHub EventHub { get; }

    public AzSynapseKustoDataConnectionIotHub IotHub { get; }

    public async Task<CommandResult> Delete(AzSynapseKustoDataConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDataConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSynapseKustoDataConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseKustoDataConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDataConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSynapseKustoDataConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDataConnectionWaitOptions(), token);
    }
}