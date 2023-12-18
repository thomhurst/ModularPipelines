using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzMobileNetwork
{
    public AzMobileNetwork(
        AzMobileNetworkAttachedDataNetwork attachedDataNetwork,
        AzMobileNetworkDataNetwork dataNetwork,
        AzMobileNetworkPccp pccp,
        AzMobileNetworkPcdp pcdp,
        AzMobileNetworkService service,
        AzMobileNetworkSim sim,
        AzMobileNetworkSite site,
        AzMobileNetworkSlice slice,
        ICommand internalCommand
    )
    {
        AttachedDataNetwork = attachedDataNetwork;
        DataNetwork = dataNetwork;
        Pccp = pccp;
        Pcdp = pcdp;
        Service = service;
        Sim = sim;
        Site = site;
        Slice = slice;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMobileNetworkAttachedDataNetwork AttachedDataNetwork { get; }

    public AzMobileNetworkDataNetwork DataNetwork { get; }

    public AzMobileNetworkPccp Pccp { get; }

    public AzMobileNetworkPcdp Pcdp { get; }

    public AzMobileNetworkService Service { get; }

    public AzMobileNetworkSim Sim { get; }

    public AzMobileNetworkSite Site { get; }

    public AzMobileNetworkSlice Slice { get; }

    public async Task<CommandResult> Create(AzMobileNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkWaitOptions(), token);
    }
}