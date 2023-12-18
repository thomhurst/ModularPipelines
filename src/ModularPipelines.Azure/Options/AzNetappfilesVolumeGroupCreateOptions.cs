using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume-group", "create")]
public record AzNetappfilesVolumeGroupCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--ppg")] string Ppg,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet")] string Vnet
) : AzOptions
{
    [CommandSwitch("--add-snapshot-capacity")]
    public string? AddSnapshotCapacity { get; set; }

    [BooleanCommandSwitch("--backup-nfsv3")]
    public bool? BackupNfsv3 { get; set; }

    [CommandSwitch("--data-backup-repl-skd")]
    public string? DataBackupReplSkd { get; set; }

    [CommandSwitch("--data-backup-size")]
    public string? DataBackupSize { get; set; }

    [CommandSwitch("--data-backup-src-id")]
    public string? DataBackupSrcId { get; set; }

    [CommandSwitch("--data-backup-throughput")]
    public string? DataBackupThroughput { get; set; }

    [CommandSwitch("--data-repl-skd")]
    public string? DataReplSkd { get; set; }

    [CommandSwitch("--data-size")]
    public string? DataSize { get; set; }

    [CommandSwitch("--data-src-id")]
    public string? DataSrcId { get; set; }

    [CommandSwitch("--data-throughput")]
    public string? DataThroughput { get; set; }

    [CommandSwitch("--global-placement-rules")]
    public string? GlobalPlacementRules { get; set; }

    [CommandSwitch("--kv-private-endpoint-id")]
    public string? KvPrivateEndpointId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-backup-repl-skd")]
    public string? LogBackupReplSkd { get; set; }

    [CommandSwitch("--log-backup-size")]
    public string? LogBackupSize { get; set; }

    [CommandSwitch("--log-backup-src-id")]
    public string? LogBackupSrcId { get; set; }

    [CommandSwitch("--log-backup-throughput")]
    public string? LogBackupThroughput { get; set; }

    [CommandSwitch("--log-size")]
    public string? LogSize { get; set; }

    [CommandSwitch("--log-throughput")]
    public string? LogThroughput { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--number-of-hots")]
    public string? NumberOfHots { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--sap-sid")]
    public string? SapSid { get; set; }

    [CommandSwitch("--shared-repl-skd")]
    public string? SharedReplSkd { get; set; }

    [CommandSwitch("--shared-size")]
    public string? SharedSize { get; set; }

    [CommandSwitch("--shared-src-id")]
    public string? SharedSrcId { get; set; }

    [CommandSwitch("--shared-throughput")]
    public string? SharedThroughput { get; set; }

    [CommandSwitch("--smb-access")]
    public string? SmbAccess { get; set; }

    [CommandSwitch("--smb-browsable")]
    public string? SmbBrowsable { get; set; }

    [CommandSwitch("--start-host-id")]
    public string? StartHostId { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--system-role")]
    public string? SystemRole { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}