using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "locations", "describe")]
public record GcloudNetappLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;