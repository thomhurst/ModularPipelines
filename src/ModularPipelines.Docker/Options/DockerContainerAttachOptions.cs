using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerAttachOptions : DockerOptions
{
    public DockerContainerAttachOptions(
        string container
    )
    {
        CommandParts = ["container", "attach"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [CommandSwitch("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [CommandSwitch("--no-stdin")]
    public virtual string? NoStdin { get; set; }

    [BooleanCommandSwitch("--sig-proxy")]
    public virtual bool? SigProxy { get; set; }
}
