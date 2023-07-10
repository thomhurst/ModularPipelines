using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose")]
public record DockerComposeOptions : DockerOptions
{
    [CommandLongSwitch("ansi")]
    public string? Ansi { get; set; }

    [CommandLongSwitch("compatibility")]
    public string? Compatibility { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("env-file")]
    public string? EnvFile { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [CommandLongSwitch("parallel")]
    public string? Parallel { get; set; }

    [CommandLongSwitch("profile")]
    public string? Profile { get; set; }

    [CommandLongSwitch("progress")]
    public string? Progress { get; set; }

    [CommandLongSwitch("project-directory")]
    public string? ProjectDirectory { get; set; }

    [CommandLongSwitch("project-name")]
    public string? ProjectName { get; set; }

}