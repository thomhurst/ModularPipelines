using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx prune")]
public record DockerBuildxPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--keep-storage")]
    public string? KeepStorage { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

}