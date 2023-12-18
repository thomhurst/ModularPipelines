using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkPublicIp
{
    public AzNetworkPublicIp(
        AzNetworkPublicIpPrefix prefix,
        ICommand internalCommand
    )
    {
        Prefix = prefix;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPublicIpPrefix Prefix { get; }

    public async Task<CommandResult> Create(AzNetworkPublicIpCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPublicIpDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPublicIpListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPublicIpShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPublicIpUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPublicIpWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPublicIpWaitOptions(), token);
    }
}