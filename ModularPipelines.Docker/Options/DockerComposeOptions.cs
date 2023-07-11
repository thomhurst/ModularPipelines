using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose")]
public record DockerComposeOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{

    [CommandSwitch("--ansi")]
    public string? Ansi { get; set; }

    [CommandSwitch("--compatibility")]
    public string? Compatibility { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--env-file")]
    public string? EnvFile { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--parallel")]
    public int? Parallel { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--project-directory")]
    public string? ProjectDirectory { get; set; }

    [CommandSwitch("--project-name")]
    public string? ProjectName { get; set; }
}