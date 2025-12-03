using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "enroll")]
public record GcloudContainerVmwareClustersEnrollOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--admin-cluster-membership")] string AdminClusterMembership,
[property: CliOption("--admin-cluster-membership-location")] string AdminClusterMembershipLocation,
[property: CliOption("--admin-cluster-membership-project")] string AdminClusterMembershipProject
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--local-name")]
    public string? LocalName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}