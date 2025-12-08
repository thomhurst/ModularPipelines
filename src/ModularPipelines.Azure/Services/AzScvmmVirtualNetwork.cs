using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm")]
public class AzScvmmVirtualNetwork
{
    public AzScvmmVirtualNetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzScvmmVirtualNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmVirtualNetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVirtualNetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzScvmmVirtualNetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVirtualNetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzScvmmVirtualNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVirtualNetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmVirtualNetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVirtualNetworkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmVirtualNetworkWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}