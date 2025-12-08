using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("compute")]
public class GcloudComputeNetworks
{
    public GcloudComputeNetworks(
        GcloudComputeNetworksPeerings peerings,
        GcloudComputeNetworksSubnets subnets,
        GcloudComputeNetworksVpcAccess vpcAccess,
        ICommand internalCommand
    )
    {
        Peerings = peerings;
        Subnets = subnets;
        VpcAccess = vpcAccess;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeNetworksPeerings Peerings { get; }

    public GcloudComputeNetworksSubnets Subnets { get; }

    public GcloudComputeNetworksVpcAccess VpcAccess { get; }

    public async Task<CommandResult> Create(GcloudComputeNetworksCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeNetworksDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeNetworksDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEffectiveFirewalls(GcloudComputeNetworksGetEffectiveFirewallsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeNetworksListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeNetworksUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}