using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "public-ip")]
public class AzNetworkPublicIpPrefix
{
    public AzNetworkPublicIpPrefix(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkPublicIpPrefixCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPublicIpPrefixDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpPrefixDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPublicIpPrefixListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpPrefixListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPublicIpPrefixShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpPrefixShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPublicIpPrefixUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpPrefixUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPublicIpPrefixWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpPrefixWaitOptions(), token);
    }
}

