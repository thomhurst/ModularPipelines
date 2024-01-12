using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "clusters", "enroll")]
public record GcloudContainerBareMetalClustersEnrollOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--admin-cluster-membership")] string AdminClusterMembership,
[property: CommandSwitch("--admin-cluster-membership-location")] string AdminClusterMembershipLocation,
[property: CommandSwitch("--admin-cluster-membership-project")] string AdminClusterMembershipProject
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}