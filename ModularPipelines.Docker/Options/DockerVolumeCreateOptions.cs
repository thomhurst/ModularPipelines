using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume create")]
public record DockerVolumeCreateOptions : DockerOptions
{
    [CommandLongSwitch("availability")]
    public string? Availability { get; set; }

    [CommandLongSwitch("driver")]
    public string? Driver { get; set; }

    [CommandLongSwitch("group")]
    public string? Group { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("limit-bytes")]
    public string? LimitBytes { get; set; }

    [CommandLongSwitch("required-bytes")]
    public string? RequiredBytes { get; set; }

    [CommandLongSwitch("scope")]
    public string? Scope { get; set; }

    [CommandLongSwitch("secret")]
    public string? Secret { get; set; }

    [CommandLongSwitch("sharing")]
    public string? Sharing { get; set; }

    [CommandLongSwitch("topology-preferred")]
    public string? TopologyPreferred { get; set; }

    [CommandLongSwitch("topology-required")]
    public string? TopologyRequired { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}