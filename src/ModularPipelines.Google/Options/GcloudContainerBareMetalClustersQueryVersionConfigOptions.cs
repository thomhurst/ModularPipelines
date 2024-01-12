using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "clusters", "query-version-config")]
public record GcloudContainerBareMetalClustersQueryVersionConfigOptions : GcloudOptions
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