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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Volume { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}
