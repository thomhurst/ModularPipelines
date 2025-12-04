using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "share", "list")]
public record AzStorageShareListOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliFlag("--include-metadata")]
    public bool? IncludeMetadata { get; set; }

    [CliFlag("--include-snapshots")]
    public bool? IncludeSnapshots { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--num-results")]
    public string? NumResults { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}