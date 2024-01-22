using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout")]
[ExcludeFromCodeCoverage]
public record DockerScoutOptions : DockerOptions
{
    [CommandSwitch("--verbose-debug")]
    public string? VerboseDebug { get; set; }
}
