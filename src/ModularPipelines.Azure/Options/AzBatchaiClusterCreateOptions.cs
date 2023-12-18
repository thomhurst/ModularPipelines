using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "cluster", "create")]
public record AzBatchaiClusterCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--afs-mount-path")]
    public string? AfsMountPath { get; set; }

    [CommandSwitch("--afs-name")]
    public string? AfsName { get; set; }

    [CommandSwitch("--bfs-mount-path")]
    public string? BfsMountPath { get; set; }

    [CommandSwitch("--bfs-name")]
    public string? BfsName { get; set; }

    [CommandSwitch("--config-file")]
    public string? ConfigFile { get; set; }

    [CommandSwitch("--custom-image")]
    public string? CustomImage { get; set; }

    [CommandSwitch("--generate-ssh-keys")]
    public string? GenerateSshKeys { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max")]
    public string? Max { get; set; }

    [CommandSwitch("--min")]
    public string? Min { get; set; }

    [CommandSwitch("--nfs")]
    public string? Nfs { get; set; }

    [CommandSwitch("--nfs-mount-path")]
    public string? NfsMountPath { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--setup-task")]
    public string? SetupTask { get; set; }

    [CommandSwitch("--setup-task-output")]
    public string? SetupTaskOutput { get; set; }

    [CommandSwitch("--ssh-key")]
    public string? SshKey { get; set; }

    [CommandSwitch("--storage-account-key")]
    public int? StorageAccountKey { get; set; }

    [CommandSwitch("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [BooleanCommandSwitch("--use-auto-storage")]
    public bool? UseAutoStorage { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--vm-priority")]
    public string? VmPriority { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }
}