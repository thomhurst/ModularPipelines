using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "versions", "set-default")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAiPlatformVersionsSetDefaultOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
