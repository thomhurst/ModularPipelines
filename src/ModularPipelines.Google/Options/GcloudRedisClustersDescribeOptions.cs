using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "describe")]
public record GcloudRedisClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;