using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "download-batch")]
public record AzStorageFileDownloadBatchOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliFlag("--dryrun")]
    public bool? Dryrun { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--max-connections")]
    public string? MaxConnections { get; set; }

    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }

    [CliOption("--pattern")]
    public string? Pattern { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliFlag("--validate-content")]
    public bool? ValidateContent { get; set; }
}