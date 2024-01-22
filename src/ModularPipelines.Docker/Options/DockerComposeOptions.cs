using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose")]
[ExcludeFromCodeCoverage]
public record DockerComposeOptions : DockerOptions
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

    [CommandSwitch("--no-ansi")]
    public string? NoAnsi { get; set; }

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

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }
}
