using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "restart")]
[ExcludeFromCodeCoverage]
public record DockerComposeRestartOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliFlag("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}
