using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkBastion
{
    public AzNetworkBastion(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkBastionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkBastionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkBastionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Rdp(AzNetworkBastionRdpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionRdpOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkBastionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Ssh(AzNetworkBastionSshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Tunnel(AzNetworkBastionTunnelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkBastionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkBastionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkBastionWaitOptions(), cancellationToken: token);
    }
}