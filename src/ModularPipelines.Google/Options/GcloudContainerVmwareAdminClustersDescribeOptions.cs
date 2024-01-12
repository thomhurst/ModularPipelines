using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "admin-clusters", "describe")]
public record GcloudContainerVmwareAdminClustersDescribeOptions(
[property: PositionalArgument] string AdminCluster,
[property: PositionalArgument] string Location
) : GcloudOptions;