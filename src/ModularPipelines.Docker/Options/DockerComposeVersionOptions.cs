using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "version")]
[ExcludeFromCodeCoverage]
public record DockerComposeVersionOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--short")]
    public virtual string? Short { get; set; }
}
