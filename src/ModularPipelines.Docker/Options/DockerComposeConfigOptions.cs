using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "config")]
[ExcludeFromCodeCoverage]
public record DockerComposeConfigOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--hash")]
    public virtual bool? Hash { get; set; }

    [CliOption("--images")]
    public virtual string? Images { get; set; }

    [CliFlag("--no-consistency")]
    public virtual bool? NoConsistency { get; set; }

    [CliFlag("--no-interpolate")]
    public virtual bool? NoInterpolate { get; set; }

    [CliFlag("--no-normalize")]
    public virtual bool? NoNormalize { get; set; }

    [CliFlag("--no-path-resolution")]
    public virtual bool? NoPathResolution { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--profiles")]
    public virtual string? Profiles { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--resolve-image-digests")]
    public virtual string? ResolveImageDigests { get; set; }

    [CliOption("--services")]
    public virtual string? Services { get; set; }

    [CliOption("--volumes")]
    public virtual string? Volumes { get; set; }
}
