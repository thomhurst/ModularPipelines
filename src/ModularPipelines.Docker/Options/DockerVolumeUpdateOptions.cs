using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("volume", "update")]
[ExcludeFromCodeCoverage]
public record DockerVolumeUpdateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Volume { get; set; }

    [CliOption("--availability")]
    public virtual string? Availability { get; set; }
}
