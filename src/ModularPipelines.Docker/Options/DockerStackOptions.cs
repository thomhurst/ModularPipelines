using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack")]
[ExcludeFromCodeCoverage]
public record DockerStackOptions : DockerOptions
{
    [CommandSwitch("--orchestrator")]
    public virtual string? Orchestrator { get; set; }
}
