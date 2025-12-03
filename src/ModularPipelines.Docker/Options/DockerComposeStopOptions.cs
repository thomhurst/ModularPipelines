using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "stop")]
[ExcludeFromCodeCoverage]
public record DockerComposeStopOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}
