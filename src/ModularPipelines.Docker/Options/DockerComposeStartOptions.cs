using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "start")]
[ExcludeFromCodeCoverage]
public record DockerComposeStartOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }
}
