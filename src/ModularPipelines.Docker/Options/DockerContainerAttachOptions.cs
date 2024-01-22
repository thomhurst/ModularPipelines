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
    public string? DetachKeys { get; set; }

    [CommandSwitch("--no-stdin")]
    public string? NoStdin { get; set; }

    [BooleanCommandSwitch("--sig-proxy")]
    public bool? SigProxy { get; set; }
}
