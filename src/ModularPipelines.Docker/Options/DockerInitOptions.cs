using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("init")]
[ExcludeFromCodeCoverage]
public record DockerInitOptions : DockerOptions
{
    [CliOption("--version")]
    public virtual string? Version { get; set; }
}
