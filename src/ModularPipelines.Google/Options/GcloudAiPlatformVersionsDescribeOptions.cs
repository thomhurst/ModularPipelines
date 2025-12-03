using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "versions", "describe")]
public record GcloudAiPlatformVersionsDescribeOptions : GcloudOptions
{
    public GcloudAiPlatformVersionsDescribeOptions(
        string version,
        string model
    )
    {
        GcloudAiPlatformVersionsDescribeOptionsVersion = version;
        Model = model;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAiPlatformVersionsDescribeOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
