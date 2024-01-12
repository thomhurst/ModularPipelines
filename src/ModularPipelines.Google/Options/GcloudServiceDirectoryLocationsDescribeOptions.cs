using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "locations", "describe")]
public record GcloudServiceDirectoryLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;