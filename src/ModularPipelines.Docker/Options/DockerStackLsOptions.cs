using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack", "ls")]
[ExcludeFromCodeCoverage]
public record DockerStackLsOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }
}
