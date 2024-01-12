using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "copy")]
public record GcloudSpannerBackupsCopyOptions(
[property: CommandSwitch("--destination-backup")] string DestinationBackup,
[property: CommandSwitch("--destination-instance")] string DestinationInstance,
[property: CommandSwitch("--expiration-date")] string ExpirationDate,
[property: CommandSwitch("--retention-period")] string RetentionPeriod,
[property: CommandSwitch("--source-backup")] string SourceBackup,
[property: CommandSwitch("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}