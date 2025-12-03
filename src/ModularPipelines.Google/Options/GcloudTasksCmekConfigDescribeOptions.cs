using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "cmek-config", "describe")]
public record GcloudTasksCmekConfigDescribeOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;