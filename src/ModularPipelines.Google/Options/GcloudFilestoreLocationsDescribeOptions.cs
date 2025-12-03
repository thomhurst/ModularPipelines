using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "locations", "describe")]
public record GcloudFilestoreLocationsDescribeOptions(
[property: CliArgument] string Zone
) : GcloudOptions;