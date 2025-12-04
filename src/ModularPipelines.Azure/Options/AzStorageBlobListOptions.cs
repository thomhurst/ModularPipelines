using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "list")]
public record AzStorageBlobListOptions(
[property: CliOption("--container-name")] string ContainerName
) : AzOptions
{
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

    [CliOption("--delimiter")]
    public string? Delimiter { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--num-results")]
    public string? NumResults { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}