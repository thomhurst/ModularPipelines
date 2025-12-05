using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "admin-clusters", "describe")]
public record GcloudContainerVmwareAdminClustersDescribeOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location
) : GcloudOptions;