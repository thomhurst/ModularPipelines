using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVirtualAppliance
{
    public AzNetworkVirtualAppliance(
        AzNetworkVirtualApplianceSite site,
        AzNetworkVirtualApplianceSku sku,
        ICommand internalCommand
    )
    {
        Site = site;
        Sku = sku;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVirtualApplianceSite Site { get; }

    public AzNetworkVirtualApplianceSku Sku { get; }

    public async Task<CommandResult> Create(AzNetworkVirtualApplianceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVirtualApplianceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVirtualApplianceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVirtualApplianceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVirtualApplianceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVirtualApplianceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceWaitOptions(), token);
    }
}