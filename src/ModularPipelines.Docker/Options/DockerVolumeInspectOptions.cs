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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Volume { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }
}
