using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto")]
public class AzKustoManagedPrivateEndpoint
{
    public AzKustoManagedPrivateEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoManagedPrivateEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoManagedPrivateEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoManagedPrivateEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzKustoManagedPrivateEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoManagedPrivateEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoManagedPrivateEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoManagedPrivateEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoManagedPrivateEndpointUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoManagedPrivateEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoManagedPrivateEndpointWaitOptions(), token);
    }
}