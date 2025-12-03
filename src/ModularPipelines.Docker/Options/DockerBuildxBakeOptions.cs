using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "bake")]
[ExcludeFromCodeCoverage]
public record DockerBuildxBakeOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Target { get; set; }

    [CliOption("--file")]
    public virtual string? File { get; set; }

    [CliOption("--load")]
    public virtual string? Load { get; set; }

    [CliOption("--metadata-file")]
    public virtual string? MetadataFile { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--print")]
    public virtual string? Print { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliOption("--provenance")]
    public virtual string? Provenance { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--push")]
    public virtual bool? Push { get; set; }

    [CliFlag("--sbom")]
    public virtual bool? Sbom { get; set; }

    [CliOption("--set")]
    public virtual string? Set { get; set; }
}
