using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image", "load")]
[ExcludeFromCodeCoverage]
public record DockerImageLoadOptions : DockerOptions
{
    [CommandSwitch("--input")]
    public virtual string? Input { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
