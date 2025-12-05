using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "version")]
[ExcludeFromCodeCoverage]
public record DockerComposeVersionOptions : DockerOptions
{
    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--short")]
    public virtual string? Short { get; set; }
}
