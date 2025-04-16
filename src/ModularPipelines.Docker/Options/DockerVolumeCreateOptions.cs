using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume", "create")]
[ExcludeFromCodeCoverage]
public record DockerVolumeCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Volume { get; set; }

    [CommandSwitch("--availability")]
    public virtual string? Availability { get; set; }

    [CommandSwitch("--driver")]
    public virtual string? Driver { get; set; }

    [CommandSwitch("--group")]
    public virtual string? Group { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--limit-bytes")]
    public virtual string? LimitBytes { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--opt")]
    public virtual string? Opt { get; set; }

    [CommandSwitch("--required-bytes")]
    public virtual string? RequiredBytes { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--secret")]
    public virtual string? Secret { get; set; }

    [CommandSwitch("--sharing")]
    public virtual string? Sharing { get; set; }

    [CommandSwitch("--topology-preferred")]
    public virtual string? TopologyPreferred { get; set; }

    [CommandSwitch("--topology-required")]
    public virtual string? TopologyRequired { get; set; }

    [CommandSwitch("--type")]
    public virtual string? Type { get; set; }
}
