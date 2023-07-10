using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose build")]
public record DockerComposeBuildOptions : DockerOptions
{
    [CommandLongSwitch("build-arg")]
    public string? BuildArg { get; set; }

    [CommandLongSwitch("memory")]
    public string? Memory { get; set; }

    [CommandLongSwitch("no-cache")]
    public string? NoCache { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("push")]
    public string? Push { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("ssh")]
    public string? Ssh { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}