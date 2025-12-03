using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("compose", "up")]
[ExcludeFromCodeCoverage]
public record DockerComposeUpOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Service { get; set; }

    [CliFlag("--abort-on-container-exit")]
    public virtual bool? AbortOnContainerExit { get; set; }

    [CliFlag("--always-recreate-deps")]
    public virtual bool? AlwaysRecreateDeps { get; set; }

    [CliFlag("--attach")]
    public virtual bool? Attach { get; set; }

    [CliOption("--attach-dependencies")]
    public virtual string? AttachDependencies { get; set; }

    [CliFlag("--build")]
    public virtual bool? Build { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--exit-code-from")]
    public virtual string? ExitCodeFrom { get; set; }

    [CliFlag("--force-recreate")]
    public virtual bool? ForceRecreate { get; set; }

    [CliOption("--no-attach")]
    public virtual string? NoAttach { get; set; }

    [CliFlag("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliFlag("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CliOption("--no-log-prefix")]
    public virtual string? NoLogPrefix { get; set; }

    [CliFlag("--no-recreate")]
    public virtual bool? NoRecreate { get; set; }

    [CliFlag("--no-start")]
    public virtual bool? NoStart { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--quiet-pull")]
    public virtual bool? QuietPull { get; set; }

    [CliFlag("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CliFlag("--renew-anon-volumes")]
    public virtual bool? RenewAnonVolumes { get; set; }

    [CliOption("--scale")]
    public virtual string? Scale { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliFlag("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliOption("--wait-timeout")]
    public virtual string? WaitTimeout { get; set; }
}
