using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "clusters", "describe")]
public record GcloudContainerAwsClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;