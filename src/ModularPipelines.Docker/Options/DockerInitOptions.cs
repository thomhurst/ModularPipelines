using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("init")]
[ExcludeFromCodeCoverage]
public record DockerInitOptions : DockerOptions
{
    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }
}
