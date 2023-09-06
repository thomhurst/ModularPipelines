using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose up")]
[ExcludeFromCodeCoverage]
public record DockerComposeUpOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? Service { get; set; }
    [BooleanCommandSwitch("--abort-on-container-exit")]
    public bool? AbortOnContainerExit { get; set; }

    [BooleanCommandSwitch("--always-recreate-deps")]
    public bool? AlwaysRecreateDeps { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandSwitch("--attach-dependencies")]
    public string? AttachDependencies { get; set; }

    [BooleanCommandSwitch("--build")]
    public bool? Build { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--exit-code-from")]
    public string? ExitCodeFrom { get; set; }

    [BooleanCommandSwitch("--force-recreate")]
    public bool? ForceRecreate { get; set; }

    [CommandSwitch("--no-attach")]
    public string? NoAttach { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public bool? NoDeps { get; set; }

    [CommandSwitch("--no-log-prefix")]
    public string? NoLogPrefix { get; set; }

    [BooleanCommandSwitch("--no-recreate")]
    public bool? NoRecreate { get; set; }

    [BooleanCommandSwitch("--no-start")]
    public bool? NoStart { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet-pull")]
    public bool? QuietPull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--renew-anon-volumes")]
    public bool? RenewAnonVolumes { get; set; }

    [CommandSwitch("--scale")]
    public string? Scale { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public bool? Timestamps { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [CommandSwitch("--wait-timeout")]
    public string? WaitTimeout { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
