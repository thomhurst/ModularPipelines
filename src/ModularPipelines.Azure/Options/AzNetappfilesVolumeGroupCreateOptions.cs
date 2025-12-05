using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume-group", "create")]
public record AzNetappfilesVolumeGroupCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--ppg")] string Ppg,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet")] string Vnet
) : AzOptions
{
    [CliOption("--add-snapshot-capacity")]
    public string? AddSnapshotCapacity { get; set; }

    [CliFlag("--backup-nfsv3")]
    public bool? BackupNfsv3 { get; set; }

    [CliOption("--data-backup-repl-skd")]
    public string? DataBackupReplSkd { get; set; }

    [CliOption("--data-backup-size")]
    public string? DataBackupSize { get; set; }

    [CliOption("--data-backup-src-id")]
    public string? DataBackupSrcId { get; set; }

    [CliOption("--data-backup-throughput")]
    public string? DataBackupThroughput { get; set; }

    [CliOption("--data-repl-skd")]
    public string? DataReplSkd { get; set; }

    [CliOption("--data-size")]
    public string? DataSize { get; set; }

    [CliOption("--data-src-id")]
    public string? DataSrcId { get; set; }

    [CliOption("--data-throughput")]
    public string? DataThroughput { get; set; }

    [CliOption("--global-placement-rules")]
    public string? GlobalPlacementRules { get; set; }

    [CliOption("--kv-private-endpoint-id")]
    public string? KvPrivateEndpointId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-backup-repl-skd")]
    public string? LogBackupReplSkd { get; set; }

    [CliOption("--log-backup-size")]
    public string? LogBackupSize { get; set; }

    [CliOption("--log-backup-src-id")]
    public string? LogBackupSrcId { get; set; }

    [CliOption("--log-backup-throughput")]
    public string? LogBackupThroughput { get; set; }

    [CliOption("--log-size")]
    public string? LogSize { get; set; }

    [CliOption("--log-throughput")]
    public string? LogThroughput { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--number-of-hots")]
    public string? NumberOfHots { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--sap-sid")]
    public string? SapSid { get; set; }

    [CliOption("--shared-repl-skd")]
    public string? SharedReplSkd { get; set; }

    [CliOption("--shared-size")]
    public string? SharedSize { get; set; }

    [CliOption("--shared-src-id")]
    public string? SharedSrcId { get; set; }

    [CliOption("--shared-throughput")]
    public string? SharedThroughput { get; set; }

    [CliOption("--smb-access")]
    public string? SmbAccess { get; set; }

    [CliOption("--smb-browsable")]
    public string? SmbBrowsable { get; set; }

    [CliOption("--start-host-id")]
    public string? StartHostId { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--system-role")]
    public string? SystemRole { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}