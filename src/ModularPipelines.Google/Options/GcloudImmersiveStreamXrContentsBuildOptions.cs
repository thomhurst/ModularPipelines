using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "contents", "build")]
public record GcloudImmersiveStreamXrContentsBuildOptions : GcloudOptions
{
    public GcloudImmersiveStreamXrContentsBuildOptions(
        string content,
        string location,
        string version
    )
    {
        Content = content;
        Location = location;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Content { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
