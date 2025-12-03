using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "imagetools", "create")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsCreateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Source { get; set; }

    [CliOption("--annotation")]
    public virtual string? Annotation { get; set; }

    [CliOption("--append")]
    public virtual string? Append { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--file")]
    public virtual string? File { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }
}
