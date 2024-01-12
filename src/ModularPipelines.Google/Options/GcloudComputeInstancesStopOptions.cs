using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "stop")]
public record GcloudComputeInstancesStopOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--discard-local-ssd[")]
    public string[]? DiscardLocalSsd { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}