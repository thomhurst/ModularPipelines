using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image", "ls")]
[ExcludeFromCodeCoverage]
public record DockerImageLsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--digests")]
    public virtual bool? Digests { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
