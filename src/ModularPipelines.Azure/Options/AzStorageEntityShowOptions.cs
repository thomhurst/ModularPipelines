using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "entity", "show")]
public record AzStorageEntityShowOptions(
[property: CliOption("--partition-key")] string PartitionKey,
[property: CliOption("--row-key")] string RowKey,
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

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}