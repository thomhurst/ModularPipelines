using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "versions", "delete")]
public record GcloudAiPlatformVersionsDeleteOptions : GcloudOptions
{
    public GcloudAiPlatformVersionsDeleteOptions(
        string version,
        string model
    )
    {
        GcloudAiPlatformVersionsDeleteOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAiPlatformVersionsDeleteOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
