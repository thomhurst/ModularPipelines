using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "create")]
public record GcloudSpannerBackupsCreateOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--expiration-date")] string ExpirationDate,
[property: CommandSwitch("--retention-period")] string RetentionPeriod
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--version-time")]
    public string? VersionTime { get; set; }

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