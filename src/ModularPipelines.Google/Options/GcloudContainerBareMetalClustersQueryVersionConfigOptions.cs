using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "query-version-config")]
public record GcloudContainerBareMetalClustersQueryVersionConfigOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--admin-cluster-membership")]
    public string? AdminClusterMembership { get; set; }

    [CliOption("--admin-cluster-membership-location")]
    public string? AdminClusterMembershipLocation { get; set; }

    [CliOption("--admin-cluster-membership-project")]
    public string? AdminClusterMembershipProject { get; set; }
}