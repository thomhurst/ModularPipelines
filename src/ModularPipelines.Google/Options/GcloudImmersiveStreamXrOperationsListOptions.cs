using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "operations", "list")]
public record GcloudImmersiveStreamXrOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}