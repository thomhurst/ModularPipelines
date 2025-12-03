using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context", "rm")]
[ExcludeFromCodeCoverage]
public record DockerContextRmOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? RmContext { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
