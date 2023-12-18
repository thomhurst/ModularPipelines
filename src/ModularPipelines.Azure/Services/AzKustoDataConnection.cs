using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto")]
public class AzKustoDataConnection
{
    public AzKustoDataConnection(
        AzKustoDataConnectionEventGrid eventGrid,
        AzKustoDataConnectionEventHub eventHub,
        AzKustoDataConnectionIotHub iotHub,
        ICommand internalCommand
    )
    {
        EventGrid = eventGrid;
        EventHub = eventHub;
        IotHub = iotHub;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKustoDataConnectionEventGrid EventGrid { get; }

    public AzKustoDataConnectionEventHub EventHub { get; }

    public AzKustoDataConnectionIotHub IotHub { get; }

    public async Task<CommandResult> Delete(AzKustoDataConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKustoDataConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoDataConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoDataConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionWaitOptions(), token);
    }
}