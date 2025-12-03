using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "service-properties", "update")]
public record AzStorageBlobServicePropertiesUpdateOptions : AzOptions
{
    [CliOption("--404-document")]
    public string? Path404Document { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--delete-retention")]
    public bool? DeleteRetention { get; set; }

    [CliOption("--delete-retention-period")]
    public string? DeleteRetentionPeriod { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--index-document")]
    public string? IndexDocument { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--static-website")]
    public bool? StaticWebsite { get; set; }
}