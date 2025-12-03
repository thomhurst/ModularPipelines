using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "describe")]
public record GcloudDataprocClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;