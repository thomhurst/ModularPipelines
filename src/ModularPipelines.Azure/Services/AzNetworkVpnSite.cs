using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVpnSite
{
    public AzNetworkVpnSite(
        AzNetworkVpnSiteLink link,
        ICommand internalCommand
    )
    {
        Link = link;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnSiteLink Link { get; }

    public async Task<CommandResult> Create(AzNetworkVpnSiteCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnSiteDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnSiteDeleteOptions(), token);
    }

    public async Task<CommandResult> Download(AzNetworkVpnSiteDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkVpnSiteListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnSiteListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnSiteShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnSiteShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVpnSiteUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnSiteUpdateOptions(), token);
    }
}