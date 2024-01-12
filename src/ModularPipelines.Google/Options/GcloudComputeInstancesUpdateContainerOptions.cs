using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "update-container")]
public record GcloudComputeInstancesUpdateContainerOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--container-mount-disk")]
    public string[]? ContainerMountDisk { get; set; }

    [BooleanCommandSwitch("--container-privileged")]
    public bool? ContainerPrivileged { get; set; }

    [CommandSwitch("--container-restart-policy")]
    public string? ContainerRestartPolicy { get; set; }

    [BooleanCommandSwitch("--container-stdin")]
    public bool? ContainerStdin { get; set; }

    [BooleanCommandSwitch("--container-tty")]
    public bool? ContainerTty { get; set; }

    [CommandSwitch("--[no-]shielded-integrity-monitoring")]
    public string[]? NoShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-learn-integrity-policy")]
    public bool? ShieldedLearnIntegrityPolicy { get; set; }

    [CommandSwitch("--[no-]shielded-secure-boot")]
    public string[]? NoShieldedSecureBoot { get; set; }

    [CommandSwitch("--[no-]shielded-vtpm")]
    public string[]? NoShieldedVtpm { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--clear-container-args")]
    public bool? ClearContainerArgs { get; set; }

    [CommandSwitch("--container-arg")]
    public string? ContainerArg { get; set; }

    [BooleanCommandSwitch("--clear-container-command")]
    public bool? ClearContainerCommand { get; set; }

    [CommandSwitch("--container-command")]
    public string? ContainerCommand { get; set; }

    [CommandSwitch("--container-env")]
    public IEnumerable<KeyValue>? ContainerEnv { get; set; }

    [CommandSwitch("--container-env-file")]
    public string? ContainerEnvFile { get; set; }

    [CommandSwitch("--remove-container-env")]
    public string[]? RemoveContainerEnv { get; set; }

    [CommandSwitch("--container-mount-host-path")]
    public string[]? ContainerMountHostPath { get; set; }

    [CommandSwitch("--container-mount-tmpfs")]
    public string[]? ContainerMountTmpfs { get; set; }

    [CommandSwitch("--remove-container-mounts")]
    public string[]? RemoveContainerMounts { get; set; }
}