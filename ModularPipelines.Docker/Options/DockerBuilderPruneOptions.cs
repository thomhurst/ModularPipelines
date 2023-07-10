using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("builder prune")]
public record DockerBuilderPruneOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("keep-storage")]
    public string? KeepStorage { get; set; }

}