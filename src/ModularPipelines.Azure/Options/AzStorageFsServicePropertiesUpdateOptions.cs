using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "fs", "service-properties", "update")]
public record AzStorageFsServicePropertiesUpdateOptions : AzOptions
{
    [CliOption("--404-document")]
    public string? Path404Document { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

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

    [CliOption("--index-document")]
    public string? IndexDocument { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliFlag("--static-website")]
    public bool? StaticWebsite { get; set; }
}