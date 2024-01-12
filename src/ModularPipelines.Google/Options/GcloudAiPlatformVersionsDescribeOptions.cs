using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "versions", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAiPlatformVersionsDescribeOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
