using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "contents", "build")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Content { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }
}
