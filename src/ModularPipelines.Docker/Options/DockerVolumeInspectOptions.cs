using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerVolumeInspectOptions : DockerOptions
{
    public DockerVolumeInspectOptions(
        IEnumerable<string> volume
    )
    {
        CommandParts = ["volume", "inspect"];

        Volume = volume;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Volume { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }
}
