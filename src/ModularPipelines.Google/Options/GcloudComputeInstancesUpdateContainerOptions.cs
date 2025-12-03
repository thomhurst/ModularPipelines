using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "update-container")]
public record GcloudComputeInstancesUpdateContainerOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--container-image")]
    public string? ContainerImage { get; set; }

    [CliOption("--container-mount-disk")]
    public string[]? ContainerMountDisk { get; set; }

    [CliFlag("--container-privileged")]
    public bool? ContainerPrivileged { get; set; }

    [CliOption("--container-restart-policy")]
    public string? ContainerRestartPolicy { get; set; }

    [CliFlag("--container-stdin")]
    public bool? ContainerStdin { get; set; }

    [CliFlag("--container-tty")]
    public bool? ContainerTty { get; set; }

    [CliOption("--[no-]shielded-integrity-monitoring")]
    public string[]? NoShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-learn-integrity-policy")]
    public bool? ShieldedLearnIntegrityPolicy { get; set; }

    [CliOption("--[no-]shielded-secure-boot")]
    public string[]? NoShieldedSecureBoot { get; set; }

    [CliOption("--[no-]shielded-vtpm")]
    public string[]? NoShieldedVtpm { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--clear-container-args")]
    public bool? ClearContainerArgs { get; set; }

    [CliOption("--container-arg")]
    public string? ContainerArg { get; set; }

    [CliFlag("--clear-container-command")]
    public bool? ClearContainerCommand { get; set; }

    [CliOption("--container-command")]
    public string? ContainerCommand { get; set; }

    [CliOption("--container-env")]
    public IEnumerable<KeyValue>? ContainerEnv { get; set; }

    [CliOption("--container-env-file")]
    public string? ContainerEnvFile { get; set; }

    [CliOption("--remove-container-env")]
    public string[]? RemoveContainerEnv { get; set; }

    [CliOption("--container-mount-host-path")]
    public string[]? ContainerMountHostPath { get; set; }

    [CliOption("--container-mount-tmpfs")]
    public string[]? ContainerMountTmpfs { get; set; }

    [CliOption("--remove-container-mounts")]
    public string[]? RemoveContainerMounts { get; set; }
}