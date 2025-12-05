using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "pause")]
[ExcludeFromCodeCoverage]
public record DockerContainerPauseOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Container { get; set; }
}
