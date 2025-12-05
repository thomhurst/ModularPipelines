using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "alpha", "dry-run")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaDryRunOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Command { get; set; }
}
