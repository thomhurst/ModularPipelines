using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "locations", "describe")]
public record GcloudSchedulerLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;