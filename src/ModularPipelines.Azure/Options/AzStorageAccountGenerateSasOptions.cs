using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "generate-sas")]
public record AzStorageAccountGenerateSasOptions(
[property: CommandSwitch("--expiry")] string Expiry,
[property: CommandSwitch("--permissions")] string Permissions,
[property: CommandSwitch("--resource-types")] string ResourceTypes,
[property: CommandSwitch("--services")] string Services
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--encryption-scope")]
    public string? EncryptionScope { get; set; }

    [CommandSwitch("--https-only")]
    public string? HttpsOnly { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}