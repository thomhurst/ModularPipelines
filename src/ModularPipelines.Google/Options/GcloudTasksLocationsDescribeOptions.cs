using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "locations", "describe")]
public record GcloudTasksLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;