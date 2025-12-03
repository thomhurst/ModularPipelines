using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "describe")]
public record GcloudContainerVmwareClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;