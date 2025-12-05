using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "restore")]
public record GcloudSpannerDatabasesRestoreOptions(
[property: CliOption("--destination-database")] string DestinationDatabase,
[property: CliOption("--destination-instance")] string DestinationInstance,
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