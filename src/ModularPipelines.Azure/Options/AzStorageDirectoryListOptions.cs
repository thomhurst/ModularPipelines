using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory", "list")]
public record AzStorageDirectoryListOptions(
[property: CommandSwitch("--share-name")] string ShareName
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

    [BooleanCommandSwitch("--exclude-extended-info")]
    public bool? ExcludeExtendedInfo { get; set; }

    [CommandSwitch("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}