using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "locations", "describe")]
public record GcloudServiceDirectoryLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;