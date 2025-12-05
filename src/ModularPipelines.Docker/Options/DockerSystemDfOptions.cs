using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("system", "df")]
[ExcludeFromCodeCoverage]
public record DockerSystemDfOptions : DockerOptions
{
    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }
}
