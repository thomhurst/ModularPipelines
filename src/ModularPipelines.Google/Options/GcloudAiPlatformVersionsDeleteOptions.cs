using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "versions", "delete")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAiPlatformVersionsDeleteOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
