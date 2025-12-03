using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "enroll")]
public record GcloudContainerBareMetalAdminClustersEnrollOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location,
[property: CliOption("--admin-cluster-membership")] string AdminClusterMembership,
[property: CliOption("--admin-cluster-membership-location")] string AdminClusterMembershipLocation
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}