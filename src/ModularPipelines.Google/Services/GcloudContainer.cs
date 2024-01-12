using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudContainer
{
    public GcloudContainer(
        GcloudContainerAttached attached,
        GcloudContainerAws aws,
        GcloudContainerAzure azure,
        GcloudContainerBareMetal bareMetal,
        GcloudContainerBinauthz binauthz,
        GcloudContainerClusters clusters,
        GcloudContainerFleet fleet,
        GcloudContainerHub hub,
        GcloudContainerImages images,
        GcloudContainerNodePools nodePools,
        GcloudContainerOperations operations,
        GcloudContainerSubnets subnets,
        GcloudContainerVmware vmware,
        ICommand internalCommand
    )
    {
        Attached = attached;
        Aws = aws;
        Azure = azure;
        BareMetal = bareMetal;
        Binauthz = binauthz;
        Clusters = clusters;
        Fleet = fleet;
        Hub = hub;
        Images = images;
        NodePools = nodePools;
        Operations = operations;
        Subnets = subnets;
        Vmware = vmware;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerAttached Attached { get; }

    public GcloudContainerAws Aws { get; }

    public GcloudContainerAzure Azure { get; }

    public GcloudContainerBareMetal BareMetal { get; }

    public GcloudContainerBinauthz Binauthz { get; }

    public GcloudContainerClusters Clusters { get; }

    public GcloudContainerFleet Fleet { get; }

    public GcloudContainerHub Hub { get; }

    public GcloudContainerImages Images { get; }

    public GcloudContainerNodePools NodePools { get; }

    public GcloudContainerOperations Operations { get; }

    public GcloudContainerSubnets Subnets { get; }

    public GcloudContainerVmware Vmware { get; }

    public async Task<CommandResult> GetServerConfig(GcloudContainerGetServerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerGetServerConfigOptions(), token);
    }
}