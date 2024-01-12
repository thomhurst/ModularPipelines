using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "cmek-config", "describe")]
public record GcloudTasksCmekConfigDescribeOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;