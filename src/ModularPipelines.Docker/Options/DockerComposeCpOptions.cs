using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposeCpOptions : DockerOptions
{
    public DockerComposeCpOptions(
        string service,
        string destPathDocker,
        string compose,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["compose", "cp"];

        Service = service;

        DestPathDocker = destPathDocker;

        Compose = compose;

        Cp = cp;

        SrcPath = srcPath;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? DestPathDocker { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Compose { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Cp { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Options { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SrcPath { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--archive")]
    public string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [CommandSwitch("--index")]
    public string? Index { get; set; }
}
