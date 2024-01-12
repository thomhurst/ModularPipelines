using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "get-screenshot")]
public record GcloudComputeInstancesGetScreenshotOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}