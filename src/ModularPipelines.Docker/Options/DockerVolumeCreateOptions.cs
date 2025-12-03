using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("volume", "create")]
[ExcludeFromCodeCoverage]
public record DockerVolumeCreateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Volume { get; set; }

    [CliOption("--availability")]
    public virtual string? Availability { get; set; }

    [CliOption("--driver")]
    public virtual string? Driver { get; set; }

    [CliOption("--group")]
    public virtual string? Group { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--limit-bytes")]
    public virtual string? LimitBytes { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--opt")]
    public virtual string? Opt { get; set; }

    [CliOption("--required-bytes")]
    public virtual string? RequiredBytes { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--secret")]
    public virtual string? Secret { get; set; }

    [CliOption("--sharing")]
    public virtual string? Sharing { get; set; }

    [CliOption("--topology-preferred")]
    public virtual string? TopologyPreferred { get; set; }

    [CliOption("--topology-required")]
    public virtual string? TopologyRequired { get; set; }

    [CliOption("--type")]
    public virtual string? Type { get; set; }
}
