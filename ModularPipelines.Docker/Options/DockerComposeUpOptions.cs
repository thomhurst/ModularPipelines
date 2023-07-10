using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose up")]
public record DockerComposeUpOptions : DockerOptions
{
    [CommandLongSwitch("abort-on-container-exit")]
    public string? AbortOnContainerExit { get; set; }

    [CommandLongSwitch("always-recreate-deps")]
    public string? AlwaysRecreateDeps { get; set; }

    [CommandLongSwitch("attach")]
    public string? Attach { get; set; }

    [CommandLongSwitch("attach-dependencies")]
    public string? AttachDependencies { get; set; }

    [CommandLongSwitch("build")]
    public string? Build { get; set; }

    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("exit-code-from")]
    public string? ExitCodeFrom { get; set; }

    [CommandLongSwitch("force-recreate")]
    public string? ForceRecreate { get; set; }

    [CommandLongSwitch("no-attach")]
    public string? NoAttach { get; set; }

    [CommandLongSwitch("no-build")]
    public string? NoBuild { get; set; }

    [CommandLongSwitch("no-color")]
    public string? NoColor { get; set; }

    [CommandLongSwitch("no-deps")]
    public string? NoDeps { get; set; }

    [CommandLongSwitch("no-log-prefix")]
    public string? NoLogPrefix { get; set; }

    [CommandLongSwitch("no-recreate")]
    public string? NoRecreate { get; set; }

    [CommandLongSwitch("no-start")]
    public string? NoStart { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("quiet-pull")]
    public string? QuietPull { get; set; }

    [CommandLongSwitch("remove-orphans")]
    public string? RemoveOrphans { get; set; }

    [CommandLongSwitch("renew-anon-volumes")]
    public string? RenewAnonVolumes { get; set; }

    [CommandLongSwitch("scale")]
    public string? Scale { get; set; }

    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("timestamps")]
    public string? Timestamps { get; set; }

    [CommandLongSwitch("wait")]
    public string? Wait { get; set; }

    [CommandLongSwitch("wait-timeout")]
    public string? WaitTimeout { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}