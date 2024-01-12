using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "admin-clusters", "enroll")]
public record GcloudContainerBareMetalAdminClustersEnrollOptions(
[property: PositionalArgument] string AdminCluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--admin-cluster-membership")] string AdminClusterMembership,
[property: CommandSwitch("--admin-cluster-membership-location")] string AdminClusterMembershipLocation
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}