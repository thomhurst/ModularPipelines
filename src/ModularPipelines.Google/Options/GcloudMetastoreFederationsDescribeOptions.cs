using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "describe")]
public record GcloudMetastoreFederationsDescribeOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location
) : GcloudOptions;