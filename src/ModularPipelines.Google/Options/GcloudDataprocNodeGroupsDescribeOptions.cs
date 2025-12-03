using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "node-groups", "describe")]
public record GcloudDataprocNodeGroupsDescribeOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;