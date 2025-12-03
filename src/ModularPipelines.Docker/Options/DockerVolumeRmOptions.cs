using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerVolumeRmOptions : DockerOptions
{
    public DockerVolumeRmOptions(
        IEnumerable<string> volume
    )
    {
        CommandParts = ["volume", "rm"];

        Volume = volume;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Volume { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
