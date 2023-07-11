using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume create")]
public record DockerVolumeCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Volume { get; set; }
    [CommandSwitch("--availability")]
    public string? Availability { get; set; }

    [CommandSwitch("--driver")]
    public string? Driver { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--limit-bytes")]
    public string? LimitBytes { get; set; }

    [CommandSwitch("--required-bytes")]
    public string? RequiredBytes { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--sharing")]
    public string? Sharing { get; set; }

    [CommandSwitch("--topology-preferred")]
    public string? TopologyPreferred { get; set; }

    [CommandSwitch("--topology-required")]
    public string? TopologyRequired { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--opt")]
    public string? Opt { get; set; }

}