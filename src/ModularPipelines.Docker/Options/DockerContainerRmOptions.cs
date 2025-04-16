using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerRmOptions : DockerOptions
{
    public DockerContainerRmOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "rm"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--link")]
    public virtual string? Link { get; set; }

    [CommandSwitch("--volumes")]
    public virtual string? Volumes { get; set; }
}
