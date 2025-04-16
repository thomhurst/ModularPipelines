using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "publish")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaPublishOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [CommandSwitch("--oci-version")]
    public virtual string? OciVersion { get; set; }

    [CommandSwitch("--resolve-image-digests")]
    public virtual string? ResolveImageDigests { get; set; }
}
