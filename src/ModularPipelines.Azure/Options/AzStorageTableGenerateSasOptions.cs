using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "table", "generate-sas")]
public record AzStorageTableGenerateSasOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--end-pk")]
    public string? EndPk { get; set; }

    [CliOption("--end-rk")]
    public string? EndRk { get; set; }

    [CliOption("--expiry")]
    public string? Expiry { get; set; }

    [CliOption("--https-only")]
    public string? HttpsOnly { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }

    [CliOption("--start-pk")]
    public string? StartPk { get; set; }

    [CliOption("--start-rk")]
    public string? StartRk { get; set; }

    [CliOption("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}