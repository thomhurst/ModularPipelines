using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerTrustInspectOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Image { get; set; }

    [CliOption("--pretty")]
    public virtual string? Pretty { get; set; }
}
