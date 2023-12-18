using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkPrivateLinkService
{
    public AzNetworkPrivateLinkService(
        AzNetworkPrivateLinkServiceConnection connection,
        ICommand internalCommand
    )
    {
        Connection = connection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPrivateLinkServiceConnection Connection { get; }

    public async Task<CommandResult> Create(AzNetworkPrivateLinkServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateLinkServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateLinkServiceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateLinkServiceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateLinkServiceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateLinkServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateLinkServiceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateLinkServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateLinkServiceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPrivateLinkServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateLinkServiceWaitOptions(), token);
    }
}

