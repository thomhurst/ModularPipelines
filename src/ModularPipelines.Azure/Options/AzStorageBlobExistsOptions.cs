using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "exists")]
public record AzStorageBlobExistsOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--blob-url")]
    public string? BlobUrl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}