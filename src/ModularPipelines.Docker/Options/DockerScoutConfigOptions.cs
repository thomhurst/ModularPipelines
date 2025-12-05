using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "config")]
[ExcludeFromCodeCoverage]
public record DockerScoutConfigOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Key { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Value { get; set; }
}
