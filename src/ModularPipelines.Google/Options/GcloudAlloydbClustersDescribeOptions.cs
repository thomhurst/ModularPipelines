using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "describe")]
public record GcloudAlloydbClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;