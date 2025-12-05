using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "admin-clusters", "enroll")]
public record GcloudContainerVmwareAdminClustersEnrollOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location,
[property: CliOption("--admin-cluster-membership")] string AdminClusterMembership,
[property: CliOption("--admin-cluster-membership-location")] string AdminClusterMembershipLocation
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}