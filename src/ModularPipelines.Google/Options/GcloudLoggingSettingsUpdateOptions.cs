using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "settings", "update")]
public record GcloudLoggingSettingsUpdateOptions(
[property: CliOption("--folder")] string Folder,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliFlag("--disable-default-sink")]
    public bool? DisableDefaultSink { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliFlag("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CliOption("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}