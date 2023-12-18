using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto")]
public class AzKustoPrivateEndpointConnection
{
    public AzKustoPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoPrivateEndpointConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoPrivateEndpointConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKustoPrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoPrivateEndpointConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoPrivateEndpointConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoPrivateEndpointConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoPrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoPrivateEndpointConnectionWaitOptions(), token);
    }
}