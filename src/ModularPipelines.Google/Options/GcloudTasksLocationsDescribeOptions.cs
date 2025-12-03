using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "locations", "describe")]
public record GcloudTasksLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;