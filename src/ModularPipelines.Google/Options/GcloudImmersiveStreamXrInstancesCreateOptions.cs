using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "instances", "create")]
public record GcloudImmersiveStreamXrInstancesCreateOptions : GcloudOptions
{
    public GcloudImmersiveStreamXrInstancesCreateOptions(
        string instance,
        string[] addRegion,
        string version,
        string content,
        string location
    )
    {
        Instance = instance;
        AddRegion = addRegion;
        Version = version;
        Content = content;
        Location = location;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Instance { get; set; }

    [CommandSwitch("--add-region")]
    public string[] AddRegion { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--content")]
    public string Content { get; set; }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--fallback-url")]
    public string? FallbackUrl { get; set; }
}
