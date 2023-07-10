using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose pull")]
public record DockerComposePullOptions : DockerOptions
{
    [CommandLongSwitch("ignore-buildable")]
    public string? IgnoreBuildable { get; set; }

    [CommandLongSwitch("ignore-pull-failures")]
    public string? IgnorePullFailures { get; set; }

    [CommandLongSwitch("include-deps")]
    public string? IncludeDeps { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}