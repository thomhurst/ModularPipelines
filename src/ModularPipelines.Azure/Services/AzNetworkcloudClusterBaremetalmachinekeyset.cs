using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cluster")]
public class AzNetworkcloudClusterBaremetalmachinekeyset
{
    public AzNetworkcloudClusterBaremetalmachinekeyset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudClusterBaremetalmachinekeysetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudClusterBaremetalmachinekeysetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBaremetalmachinekeysetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudClusterBaremetalmachinekeysetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudClusterBaremetalmachinekeysetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBaremetalmachinekeysetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudClusterBaremetalmachinekeysetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBaremetalmachinekeysetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudClusterBaremetalmachinekeysetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBaremetalmachinekeysetWaitOptions(), token);
    }
}