using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "describe")]
public record GcloudContainerVmwareClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions;