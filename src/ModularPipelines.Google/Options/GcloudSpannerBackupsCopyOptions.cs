using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "copy")]
public record GcloudSpannerBackupsCopyOptions(
[property: CliOption("--destination-backup")] string DestinationBackup,
[property: CliOption("--destination-instance")] string DestinationInstance,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod,
[property: CliOption("--source-backup")] string SourceBackup,
[property: CliOption("--source-instance")] string SourceInstance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}