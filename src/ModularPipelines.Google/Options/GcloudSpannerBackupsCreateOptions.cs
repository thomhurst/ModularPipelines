using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "create")]
public record GcloudSpannerBackupsCreateOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance,
[property: CliOption("--database")] string Database,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--version-time")]
    public string? VersionTime { get; set; }

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