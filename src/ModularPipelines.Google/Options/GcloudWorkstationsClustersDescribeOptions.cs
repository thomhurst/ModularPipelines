using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "clusters", "describe")]
public record GcloudWorkstationsClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;