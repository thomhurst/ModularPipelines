using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx prune")]
public record DockerBuildxPruneOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("keep-storage")]
    public string? KeepStorage { get; set; }

    [CommandLongSwitch("verbose")]
    public string? Verbose { get; set; }

}