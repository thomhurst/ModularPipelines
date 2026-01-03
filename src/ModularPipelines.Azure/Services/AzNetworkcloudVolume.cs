using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud")]
public class AzNetworkcloudVolume
{
    public AzNetworkcloudVolume(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudVolumeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudVolumeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVolumeDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkcloudVolumeListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVolumeListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudVolumeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVolumeShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudVolumeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVolumeUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudVolumeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudVolumeWaitOptions(), cancellationToken: token);
    }
}