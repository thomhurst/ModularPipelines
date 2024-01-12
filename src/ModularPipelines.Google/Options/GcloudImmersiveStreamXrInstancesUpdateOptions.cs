using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "instances", "update")]
public record GcloudImmersiveStreamXrInstancesUpdateOptions : GcloudOptions
{
    public GcloudImmersiveStreamXrInstancesUpdateOptions(
        string instance,
        string location,
        string[] addRegion,
        string fallbackUrl,
        string removeRegion,
        string[] updateRegion,
        string version
    )
    {
        Instance = instance;
        Location = location;
        AddRegion = addRegion;
        FallbackUrl = fallbackUrl;
        RemoveRegion = removeRegion;
        UpdateRegion = updateRegion;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Instance { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--add-region")]
    public string[] AddRegion { get; set; }

    [CommandSwitch("--fallback-url")]
    public string FallbackUrl { get; set; }

    [CommandSwitch("--remove-region")]
    public string RemoveRegion { get; set; }

    [CommandSwitch("--update-region")]
    public string[] UpdateRegion { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
