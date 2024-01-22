using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system", "info")]
[ExcludeFromCodeCoverage]
public record DockerSystemInfoOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
