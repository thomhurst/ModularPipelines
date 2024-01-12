using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "settings", "update")]
public record GcloudLoggingSettingsUpdateOptions(
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--disable-default-sink")]
    public bool? DisableDefaultSink { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [BooleanCommandSwitch("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CommandSwitch("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}