using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "generate-sas")]
public record AzStorageAccountGenerateSasOptions(
[property: CliOption("--expiry")] string Expiry,
[property: CliOption("--permissions")] string Permissions,
[property: CliOption("--resource-types")] string ResourceTypes,
[property: CliOption("--services")] string Services
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--encryption-scope")]
    public string? EncryptionScope { get; set; }

    [CliOption("--https-only")]
    public string? HttpsOnly { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}