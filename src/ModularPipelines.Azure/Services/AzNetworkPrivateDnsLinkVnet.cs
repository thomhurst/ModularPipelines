using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-dns", "link")]
public class AzNetworkPrivateDnsLinkVnet
{
    public AzNetworkPrivateDnsLinkVnet(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkPrivateDnsLinkVnetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateDnsLinkVnetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsLinkVnetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateDnsLinkVnetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateDnsLinkVnetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsLinkVnetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateDnsLinkVnetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsLinkVnetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPrivateDnsLinkVnetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsLinkVnetWaitOptions(), token);
    }
}