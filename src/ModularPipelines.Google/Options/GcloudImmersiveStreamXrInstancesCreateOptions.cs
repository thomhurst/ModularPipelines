using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "instances", "create")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Instance { get; set; }

    [CliOption("--add-region")]
    public string[] AddRegion { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliOption("--content")]
    public string Content { get; set; }

    [CliOption("--location")]
    public string Location { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--fallback-url")]
    public string? FallbackUrl { get; set; }
}
