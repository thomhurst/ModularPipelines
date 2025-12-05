using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "wait")]
[ExcludeFromCodeCoverage]
public record DockerComposeWaitOptions : DockerOptions
{
    [CliOption("--down-project")]
    public virtual string? DownProject { get; set; }
}
