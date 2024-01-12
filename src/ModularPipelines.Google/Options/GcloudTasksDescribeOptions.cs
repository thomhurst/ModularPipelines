using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "describe")]
public record GcloudTasksDescribeOptions(
[property: PositionalArgument] string Task
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--queue")]
    public string? Queue { get; set; }

    [CommandSwitch("--response-view")]
    public string? ResponseView { get; set; }
}