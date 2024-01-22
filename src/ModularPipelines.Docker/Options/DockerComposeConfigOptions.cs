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
    public string? Format { get; set; }

    [BooleanCommandSwitch("--hash")]
    public bool? Hash { get; set; }

    [CommandSwitch("--images")]
    public string? Images { get; set; }

    [BooleanCommandSwitch("--no-consistency")]
    public bool? NoConsistency { get; set; }

    [BooleanCommandSwitch("--no-interpolate")]
    public bool? NoInterpolate { get; set; }

    [BooleanCommandSwitch("--no-normalize")]
    public bool? NoNormalize { get; set; }

    [BooleanCommandSwitch("--no-path-resolution")]
    public bool? NoPathResolution { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--profiles")]
    public string? Profiles { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--resolve-image-digests")]
    public string? ResolveImageDigests { get; set; }

    [CommandSwitch("--services")]
    public string? Services { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }
}
