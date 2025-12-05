using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("network", "rm")]
[ExcludeFromCodeCoverage]
public record DockerNetworkRmOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Network { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
