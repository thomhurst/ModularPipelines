using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "custom-ip")]
public class AzNetworkCustomIpPrefix
{
    public AzNetworkCustomIpPrefix(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkCustomIpPrefixCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkCustomIpPrefixDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCustomIpPrefixDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkCustomIpPrefixListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCustomIpPrefixListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkCustomIpPrefixShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCustomIpPrefixShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkCustomIpPrefixUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCustomIpPrefixUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkCustomIpPrefixWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCustomIpPrefixWaitOptions(), token);
    }
}