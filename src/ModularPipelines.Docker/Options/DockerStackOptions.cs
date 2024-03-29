using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack")]
[ExcludeFromCodeCoverage]
public record DockerStackOptions : DockerOptions
{
    [CommandSwitch("--orchestrator")]
    public string? Orchestrator { get; set; }
}
