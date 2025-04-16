using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose")]
[ExcludeFromCodeCoverage]
public record DockerComposeOptions : DockerOptions
{
    [CommandSwitch("--ansi")]
    public virtual string? Ansi { get; set; }

    [CommandSwitch("--compatibility")]
    public virtual string? Compatibility { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandSwitch("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CommandSwitch("--file")]
    public virtual string? File { get; set; }

    [CommandSwitch("--no-ansi")]
    public virtual string? NoAnsi { get; set; }

    [CommandSwitch("--parallel")]
    public virtual int? Parallel { get; set; }

    [CommandSwitch("--profile")]
    public virtual string? Profile { get; set; }

    [CommandSwitch("--progress")]
    public virtual string? Progress { get; set; }

    [CommandSwitch("--project-directory")]
    public virtual string? ProjectDirectory { get; set; }

    [CommandSwitch("--project-name")]
    public virtual string? ProjectName { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
