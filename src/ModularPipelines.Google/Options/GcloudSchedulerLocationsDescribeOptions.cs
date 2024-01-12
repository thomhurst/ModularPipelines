using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "locations", "describe")]
public record GcloudSchedulerLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;