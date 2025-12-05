using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerContextInspectOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? InspectContext { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }
}
