using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose run")]
public record DockerComposeRunOptions : DockerOptions
{
    [CommandLongSwitch("build")]
    public string? Build { get; set; }

    [CommandLongSwitch("cap-add")]
    public string? CapAdd { get; set; }

    [CommandLongSwitch("cap-drop")]
    public string? CapDrop { get; set; }

    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandLongSwitch("env")]
    public string? Env { get; set; }

    [BooleanCommandSwitch("interactive")]
    public bool? Interactive { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("no-TTY")]
    public bool? NoTTY { get; set; }

    [CommandLongSwitch("no-deps")]
    public string? NoDeps { get; set; }

    [CommandLongSwitch("publish")]
    public string? Publish { get; set; }

    [CommandLongSwitch("quiet-pull")]
    public string? QuietPull { get; set; }

    [CommandLongSwitch("remove-orphans")]
    public string? RemoveOrphans { get; set; }

    [CommandLongSwitch("rm")]
    public string? Rm { get; set; }

    [CommandLongSwitch("service-ports")]
    public string? ServicePorts { get; set; }

    [CommandLongSwitch("use-aliases")]
    public string? UseAliases { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("volume")]
    public string? Volume { get; set; }

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}