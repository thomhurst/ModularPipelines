using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack", "ls")]
[ExcludeFromCodeCoverage]
public record DockerStackLsOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
