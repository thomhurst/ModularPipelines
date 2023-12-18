using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy", "cancel", "(storage-preview", "extension)")]
public record AzStorageFileCopyCancelStoragePreviewExtensionOptions(
[property: CommandSwitch("--copy-id")] string CopyId,
[property: CommandSwitch("--destination-path")] string DestinationPath,
[property: CommandSwitch("--destination-share")] string DestinationShare
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [BooleanCommandSwitch("--backup-intent")]
    public bool? BackupIntent { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}