using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerBuildxInspectOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }

    [CliOption("--bootstrap")]
    public virtual string? Bootstrap { get; set; }
}
