using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "job", "create")]
public record AzBatchaiJobCreateOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--experiment")] string Experiment,
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

    [CommandSwitch("--nfs")]
    public string? Nfs { get; set; }

    [CommandSwitch("--nfs-mount-path")]
    public string? NfsMountPath { get; set; }

    [CommandSwitch("--storage-account-key")]
    public int? StorageAccountKey { get; set; }

    [CommandSwitch("--storage-account-name")]
    public int? StorageAccountName { get; set; }
}