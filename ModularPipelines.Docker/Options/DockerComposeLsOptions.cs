using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose ls")]
public record DockerComposeLsOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}