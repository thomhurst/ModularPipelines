using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "key", "generate")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyGenerateOptions : DockerOptions
{
    [CliOption("--dir")]
    public virtual string? Dir { get; set; }
}
