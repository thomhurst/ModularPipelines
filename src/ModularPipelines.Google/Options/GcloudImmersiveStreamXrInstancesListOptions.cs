using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "instances", "list")]
public record GcloudImmersiveStreamXrInstancesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}