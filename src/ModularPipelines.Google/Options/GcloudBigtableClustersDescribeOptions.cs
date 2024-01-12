using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "clusters", "describe")]
public record GcloudBigtableClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance
) : GcloudOptions;