using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "enroll")]
public record GcloudContainerVmwareClustersEnrollOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--admin-cluster-membership")] string AdminClusterMembership,
[property: CommandSwitch("--admin-cluster-membership-location")] string AdminClusterMembershipLocation,
[property: CommandSwitch("--admin-cluster-membership-project")] string AdminClusterMembershipProject
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--local-name")]
    public string? LocalName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}