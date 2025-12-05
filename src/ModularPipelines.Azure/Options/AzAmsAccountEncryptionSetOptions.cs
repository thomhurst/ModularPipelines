using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "account", "encryption", "set")]
public record AzAmsAccountEncryptionSetOptions(
[property: CliOption("--key-type")] string KeyType
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--current-key-id")]
    public string? CurrentKeyId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-identifier")]
    public string? KeyIdentifier { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}