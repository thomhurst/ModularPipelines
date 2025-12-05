using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "enroll")]
public record GcloudContainerBareMetalClustersEnrollOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--admin-cluster-membership")] string AdminClusterMembership,
[property: CliOption("--admin-cluster-membership-location")] string AdminClusterMembershipLocation,
[property: CliOption("--admin-cluster-membership-project")] string AdminClusterMembershipProject
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}