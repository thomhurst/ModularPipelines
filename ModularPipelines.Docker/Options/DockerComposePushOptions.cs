using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose push")]
public record DockerComposePushOptions : DockerOptions
{
    [CommandLongSwitch("ignore-push-failures")]
    public string? IgnorePushFailures { get; set; }

    [CommandLongSwitch("include-deps")]
    public string? IncludeDeps { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}