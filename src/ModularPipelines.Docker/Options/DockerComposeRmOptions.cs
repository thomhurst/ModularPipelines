using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "rm")]
[ExcludeFromCodeCoverage]
public record DockerComposeRmOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--stop")]
    public virtual string? Stop { get; set; }

    [CliOption("--volumes")]
    public virtual string? Volumes { get; set; }
}
