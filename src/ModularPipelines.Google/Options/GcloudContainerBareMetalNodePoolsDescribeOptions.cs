using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "node-pools", "describe")]
public record GcloudContainerBareMetalNodePoolsDescribeOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;