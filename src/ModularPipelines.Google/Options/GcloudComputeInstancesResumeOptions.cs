using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "resume")]
public record GcloudComputeInstancesResumeOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}