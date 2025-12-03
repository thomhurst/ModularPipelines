using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "describe")]
public record GcloudAlloydbClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions;