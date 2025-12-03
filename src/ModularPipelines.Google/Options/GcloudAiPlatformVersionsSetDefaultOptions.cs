using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "versions", "set-default")]
public record GcloudAiPlatformVersionsSetDefaultOptions : GcloudOptions
{
    public GcloudAiPlatformVersionsSetDefaultOptions(
        string version,
        string model
    )
    {
        GcloudAiPlatformVersionsSetDefaultOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAiPlatformVersionsSetDefaultOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
