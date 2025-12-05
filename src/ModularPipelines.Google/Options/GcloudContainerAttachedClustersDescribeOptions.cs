using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "describe")]
public record GcloudContainerAttachedClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;