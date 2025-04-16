using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("version")]
[ExcludeFromCodeCoverage]
public record DockerVersionOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }
}
