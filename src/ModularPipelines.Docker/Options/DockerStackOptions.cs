using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("stack")]
[ExcludeFromCodeCoverage]
public record DockerStackOptions : DockerOptions
{
    [CliOption("--orchestrator")]
    public virtual string? Orchestrator { get; set; }
}
