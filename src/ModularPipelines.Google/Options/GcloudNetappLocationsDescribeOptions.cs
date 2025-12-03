using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "locations", "describe")]
public record GcloudNetappLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;