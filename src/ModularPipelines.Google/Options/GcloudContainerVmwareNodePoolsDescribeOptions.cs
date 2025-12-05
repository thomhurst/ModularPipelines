using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "node-pools", "describe")]
public record GcloudContainerVmwareNodePoolsDescribeOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;