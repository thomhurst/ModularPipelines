using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "table", "policy", "delete")]
public record AzStorageTablePolicyDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--table-name")] string TableName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}