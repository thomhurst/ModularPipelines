using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposeCpOptions : DockerOptions
{
    public DockerComposeCpOptions(
        string service,
        string destPath,
        string compose,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["compose", "cp"];

        Service = service;

        DestPath = destPath;

        Compose = compose;

        Cp = cp;

        SrcPath = srcPath;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? DestPath { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Compose { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Cp { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Options { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SrcPath { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--archive")]
    public virtual string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public virtual string? FollowLink { get; set; }

    [CommandSwitch("--index")]
    public virtual string? Index { get; set; }
}