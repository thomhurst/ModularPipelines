using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "clusters", "describe")]
public record GcloudBigtableClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance
) : GcloudOptions;