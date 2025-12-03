using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "contents", "list")]
public record GcloudImmersiveStreamXrContentsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}