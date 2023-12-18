using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs", "service-properties", "update")]
public record AzStorageFsServicePropertiesUpdateOptions : AzOptions
{
    [CommandSwitch("--404-document")]
    public string? _404document { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

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

    [CommandSwitch("--index-document")]
    public string? IndexDocument { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [BooleanCommandSwitch("--static-website")]
    public bool? StaticWebsite { get; set; }
}