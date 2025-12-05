using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "instances", "update")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Instance { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--add-region")]
    public string[] AddRegion { get; set; }

    [CliOption("--fallback-url")]
    public string FallbackUrl { get; set; }

    [CliOption("--remove-region")]
    public string RemoveRegion { get; set; }

    [CliOption("--update-region")]
    public string[] UpdateRegion { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }
}
