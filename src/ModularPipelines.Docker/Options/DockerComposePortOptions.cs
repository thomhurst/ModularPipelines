using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposePortOptions : DockerOptions
{
    public DockerComposePortOptions(
        string service,
        string privatePort
    )
    {
        CommandParts = ["compose", "port"];

        Service = service;

        PrivatePort = privatePort;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? PrivatePort { get; set; }

    [CommandSwitch("--index")]
    public virtual string? Index { get; set; }

    [CommandSwitch("--protocol")]
    public virtual string? Protocol { get; set; }
}
