using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "cluster")]
public class AzNetworkcloudClusterBmckeyset
{
    public AzNetworkcloudClusterBmckeyset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudClusterBmckeysetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudClusterBmckeysetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBmckeysetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudClusterBmckeysetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudClusterBmckeysetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBmckeysetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudClusterBmckeysetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBmckeysetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudClusterBmckeysetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterBmckeysetWaitOptions(), token);
    }
}