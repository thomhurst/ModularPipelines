using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "up")]
[ExcludeFromCodeCoverage]
public record DockerComposeUpOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--abort-on-container-exit")]
    public virtual bool? AbortOnContainerExit { get; set; }

    [BooleanCommandSwitch("--always-recreate-deps")]
    public virtual bool? AlwaysRecreateDeps { get; set; }

    [BooleanCommandSwitch("--attach")]
    public virtual bool? Attach { get; set; }

    [CommandSwitch("--attach-dependencies")]
    public virtual string? AttachDependencies { get; set; }

    [BooleanCommandSwitch("--build")]
    public virtual bool? Build { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--exit-code-from")]
    public virtual string? ExitCodeFrom { get; set; }

    [BooleanCommandSwitch("--force-recreate")]
    public virtual bool? ForceRecreate { get; set; }

    [CommandSwitch("--no-attach")]
    public virtual string? NoAttach { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CommandSwitch("--no-log-prefix")]
    public virtual string? NoLogPrefix { get; set; }

    [BooleanCommandSwitch("--no-recreate")]
    public virtual bool? NoRecreate { get; set; }

    [BooleanCommandSwitch("--no-start")]
    public virtual bool? NoStart { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet-pull")]
    public virtual bool? QuietPull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--renew-anon-volumes")]
    public virtual bool? RenewAnonVolumes { get; set; }

    [CommandSwitch("--scale")]
    public virtual string? Scale { get; set; }

    [CommandSwitch("--timeout")]
    public virtual string? Timeout { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [CommandSwitch("--wait-timeout")]
    public virtual string? WaitTimeout { get; set; }
}
