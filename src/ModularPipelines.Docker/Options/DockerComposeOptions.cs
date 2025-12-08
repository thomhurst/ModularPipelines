using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("compose")]
[ExcludeFromCodeCoverage]
public record DockerComposeOptions : DockerOptions
{
    [CliOption("--ansi")]
    public virtual string? Ansi { get; set; }

    [CliOption("--compatibility")]
    public virtual string? Compatibility { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CliOption("--file")]
    public virtual string? File { get; set; }

    [CliOption("--no-ansi")]
    public virtual string? NoAnsi { get; set; }

    [CliOption("--parallel")]
    public virtual int? Parallel { get; set; }

    [CliOption("--profile")]
    public virtual string? Profile { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliOption("--project-directory")]
    public virtual string? ProjectDirectory { get; set; }

    [CliOption("--project-name")]
    public virtual string? ProjectName { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
