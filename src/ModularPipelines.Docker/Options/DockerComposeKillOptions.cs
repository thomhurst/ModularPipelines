using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "kill")]
[ExcludeFromCodeCoverage]
public record DockerComposeKillOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliFlag("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CliOption("--signal")]
    public virtual string? Signal { get; set; }
}
