using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub")]
public class AzNetworkVhubBgpconnection
{
    public AzNetworkVhubBgpconnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkVhubBgpconnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkVhubBgpconnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubBgpconnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkVhubBgpconnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkVhubBgpconnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubBgpconnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkVhubBgpconnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubBgpconnectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkVhubBgpconnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVhubBgpconnectionWaitOptions(), cancellationToken: token);
    }
}