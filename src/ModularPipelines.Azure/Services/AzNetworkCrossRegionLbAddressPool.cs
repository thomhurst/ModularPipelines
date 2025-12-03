using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-region-lb")]
public class AzNetworkCrossRegionLbAddressPool
{
    public AzNetworkCrossRegionLbAddressPool(
        AzNetworkCrossRegionLbAddressPoolAddress address,
        ICommand internalCommand
    )
    {
        Address = address;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkCrossRegionLbAddressPoolAddress Address { get; }

    public async Task<CommandResult> Create(AzNetworkCrossRegionLbAddressPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkCrossRegionLbAddressPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbAddressPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkCrossRegionLbAddressPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkCrossRegionLbAddressPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbAddressPoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkCrossRegionLbAddressPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkCrossRegionLbAddressPoolUpdateOptions(), token);
    }
}