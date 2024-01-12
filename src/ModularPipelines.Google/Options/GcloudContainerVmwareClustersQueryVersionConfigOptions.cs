using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "query-version-config")]
public record GcloudContainerVmwareClustersQueryVersionConfigOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--admin-cluster-membership")]
    public string? AdminClusterMembership { get; set; }

    [CommandSwitch("--admin-cluster-membership-location")]
    public string? AdminClusterMembershipLocation { get; set; }

    [CommandSwitch("--admin-cluster-membership-project")]
    public string? AdminClusterMembershipProject { get; set; }
}