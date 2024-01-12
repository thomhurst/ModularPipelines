using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "locations", "describe")]
public record GcloudFilestoreLocationsDescribeOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions;