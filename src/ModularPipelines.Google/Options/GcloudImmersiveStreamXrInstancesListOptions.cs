using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "instances", "list")]
public record GcloudImmersiveStreamXrInstancesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}