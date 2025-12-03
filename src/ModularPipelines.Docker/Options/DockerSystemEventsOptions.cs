using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("system", "events")]
[ExcludeFromCodeCoverage]
public record DockerSystemEventsOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--since")]
    public virtual string? Since { get; set; }

    [CliOption("--until")]
    public virtual string? Until { get; set; }
}
