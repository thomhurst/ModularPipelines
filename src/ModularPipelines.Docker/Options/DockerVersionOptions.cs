using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("version")]
[ExcludeFromCodeCoverage]
public record DockerVersionOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
