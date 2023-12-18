using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "service-properties", "update")]
public record AzStorageBlobServicePropertiesUpdateOptions : AzOptions
{
    [CommandSwitch("--404-document")]
    public string? _404document { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--delete-retention")]
    public bool? DeleteRetention { get; set; }

    [CommandSwitch("--delete-retention-period")]
    public string? DeleteRetentionPeriod { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--index-document")]
    public string? IndexDocument { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--static-website")]
    public bool? StaticWebsite { get; set; }
}