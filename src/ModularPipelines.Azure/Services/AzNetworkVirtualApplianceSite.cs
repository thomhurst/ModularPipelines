using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "virtual-appliance")]
public class AzNetworkVirtualApplianceSite
{
    public AzNetworkVirtualApplianceSite(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkVirtualApplianceSiteCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkVirtualApplianceSiteDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSiteDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkVirtualApplianceSiteListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkVirtualApplianceSiteShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSiteShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkVirtualApplianceSiteUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSiteUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkVirtualApplianceSiteWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSiteWaitOptions(), cancellationToken: token);
    }
}