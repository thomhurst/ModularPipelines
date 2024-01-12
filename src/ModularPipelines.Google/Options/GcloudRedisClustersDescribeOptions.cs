using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "clusters", "describe")]
public record GcloudRedisClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;