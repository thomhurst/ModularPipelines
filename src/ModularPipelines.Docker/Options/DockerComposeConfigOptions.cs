using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "config")]
[ExcludeFromCodeCoverage]
public record DockerComposeConfigOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--hash")]
    public virtual bool? Hash { get; set; }

    [CommandSwitch("--images")]
    public virtual string? Images { get; set; }

    [BooleanCommandSwitch("--no-consistency")]
    public virtual bool? NoConsistency { get; set; }

    [BooleanCommandSwitch("--no-interpolate")]
    public virtual bool? NoInterpolate { get; set; }

    [BooleanCommandSwitch("--no-normalize")]
    public virtual bool? NoNormalize { get; set; }

    [BooleanCommandSwitch("--no-path-resolution")]
    public virtual bool? NoPathResolution { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--profiles")]
    public virtual string? Profiles { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandSwitch("--resolve-image-digests")]
    public virtual string? ResolveImageDigests { get; set; }

    [CommandSwitch("--services")]
    public virtual string? Services { get; set; }

    [CommandSwitch("--volumes")]
    public virtual string? Volumes { get; set; }
}
