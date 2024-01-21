using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "version")]
[ExcludeFromCodeCoverage]
public record DockerComposeVersionOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--short")]
    public string? Short { get; set; }
}
