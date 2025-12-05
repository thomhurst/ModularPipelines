using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "create")]
[ExcludeFromCodeCoverage]
public record DockerComposeCreateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliFlag("--build")]
    public virtual bool? Build { get; set; }

    [CliFlag("--force-recreate")]
    public virtual bool? ForceRecreate { get; set; }

    [CliFlag("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [CliFlag("--no-recreate")]
    public virtual bool? NoRecreate { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CliOption("--scale")]
    public virtual string? Scale { get; set; }
}
