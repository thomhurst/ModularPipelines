using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "virtual-appliance")]
public class AzNetworkVirtualApplianceSku
{
    public AzNetworkVirtualApplianceSku(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzNetworkVirtualApplianceSkuListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSkuListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVirtualApplianceSkuShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVirtualApplianceSkuShowOptions(), token);
    }
}