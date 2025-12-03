using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "entity", "query")]
public record AzStorageEntityQueryOptions(
[property: CliOption("--table-name")] string TableName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--num-results")]
    public string? NumResults { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}