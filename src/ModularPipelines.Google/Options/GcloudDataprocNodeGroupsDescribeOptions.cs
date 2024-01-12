using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "node-groups", "describe")]
public record GcloudDataprocNodeGroupsDescribeOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;