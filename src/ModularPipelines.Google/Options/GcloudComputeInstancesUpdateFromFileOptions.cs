using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "update-from-file")]
public record GcloudComputeInstancesUpdateFromFileOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--minimal-action")]
    public string? MinimalAction { get; set; }

    [CommandSwitch("--most-disruptive-allowed-action")]
    public string? MostDisruptiveAllowedAction { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}