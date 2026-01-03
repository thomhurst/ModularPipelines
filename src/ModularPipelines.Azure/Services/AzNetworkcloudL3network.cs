using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud")]
public class AzNetworkcloudL3network
{
    public AzNetworkcloudL3network(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudL3networkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudL3networkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL3networkDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkcloudL3networkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL3networkListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudL3networkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL3networkShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudL3networkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL3networkUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudL3networkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL3networkWaitOptions(), cancellationToken: token);
    }
}