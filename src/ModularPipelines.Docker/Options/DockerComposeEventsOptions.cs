using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "events")]
[ExcludeFromCodeCoverage]
public record DockerComposeEventsOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--json")]
    public virtual string? Json { get; set; }
}
