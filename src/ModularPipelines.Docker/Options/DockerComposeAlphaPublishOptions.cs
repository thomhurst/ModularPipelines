using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "alpha", "publish")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaPublishOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Repository { get; set; }

    [CliOption("--oci-version")]
    public virtual string? OciVersion { get; set; }

    [CliOption("--resolve-image-digests")]
    public virtual string? ResolveImageDigests { get; set; }
}
