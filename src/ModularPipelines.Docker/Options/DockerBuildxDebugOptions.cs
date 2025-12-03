using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "debug")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDebugOptions : DockerOptions
{
    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--invoke")]
    public virtual string? Invoke { get; set; }

    [CliOption("--on")]
    public virtual string? On { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliOption("--root")]
    public virtual string? Root { get; set; }

    [CliOption("--server-config")]
    public virtual string? ServerConfig { get; set; }
}
