using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkNic
{
    public AzNetworkNic(
        AzNetworkNicIpConfig ipConfig,
        AzNetworkNicVtapConfig vtapConfig,
        ICommand internalCommand
    )
    {
        IpConfig = ipConfig;
        VtapConfig = vtapConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkNicIpConfig IpConfig { get; }

    public AzNetworkNicVtapConfig VtapConfig { get; }

    public async Task<CommandResult> Create(AzNetworkNicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkNicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkNicListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicListOptions(), token);
    }

    public async Task<CommandResult> ListEffectiveNsg(AzNetworkNicListEffectiveNsgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicListEffectiveNsgOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkNicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicShowOptions(), token);
    }

    public async Task<CommandResult> ShowEffectiveRouteTable(AzNetworkNicShowEffectiveRouteTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicShowEffectiveRouteTableOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkNicUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkNicWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicWaitOptions(), token);
    }
}