using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class GcloudVmwarePrivateClouds
{
    public GcloudVmwarePrivateClouds(
        GcloudVmwarePrivateCloudsClusters clusters,
        GcloudVmwarePrivateCloudsDnsForwarding dnsForwarding,
        GcloudVmwarePrivateCloudsExternalAddresses externalAddresses,
        GcloudVmwarePrivateCloudsHcx hcx,
        GcloudVmwarePrivateCloudsLoggingServers loggingServers,
        GcloudVmwarePrivateCloudsManagementDnsZoneBindings managementDnsZoneBindings,
        GcloudVmwarePrivateCloudsNsx nsx,
        GcloudVmwarePrivateCloudsSubnets subnets,
        GcloudVmwarePrivateCloudsVcenter vcenter,
        ICommand internalCommand
    )
    {
        Clusters = clusters;
        DnsForwarding = dnsForwarding;
        ExternalAddresses = externalAddresses;
        Hcx = hcx;
        LoggingServers = loggingServers;
        ManagementDnsZoneBindings = managementDnsZoneBindings;
        Nsx = nsx;
        Subnets = subnets;
        Vcenter = vcenter;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudVmwarePrivateCloudsClusters Clusters { get; }

    public GcloudVmwarePrivateCloudsDnsForwarding DnsForwarding { get; }

    public GcloudVmwarePrivateCloudsExternalAddresses ExternalAddresses { get; }

    public GcloudVmwarePrivateCloudsHcx Hcx { get; }

    public GcloudVmwarePrivateCloudsLoggingServers LoggingServers { get; }

    public GcloudVmwarePrivateCloudsManagementDnsZoneBindings ManagementDnsZoneBindings { get; }

    public GcloudVmwarePrivateCloudsNsx Nsx { get; }

    public GcloudVmwarePrivateCloudsSubnets Subnets { get; }

    public GcloudVmwarePrivateCloudsVcenter Vcenter { get; }

    public async Task<CommandResult> Create(GcloudVmwarePrivateCloudsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudVmwarePrivateCloudsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudVmwarePrivateCloudsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudVmwarePrivateCloudsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudVmwarePrivateCloudsListOptions(), token);
    }

    public async Task<CommandResult> Undelete(GcloudVmwarePrivateCloudsUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudVmwarePrivateCloudsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}