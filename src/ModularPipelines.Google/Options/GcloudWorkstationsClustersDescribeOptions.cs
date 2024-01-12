using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "clusters", "describe")]
public record GcloudWorkstationsClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;