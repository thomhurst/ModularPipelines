using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "update")]
public record AzNetappfilesAccountUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-source")]
    public string? KeySource { get; set; }

    [CliOption("--key-vault-uri")]
    public string? KeyVaultUri { get; set; }

    [CliOption("--keyvault-resource-id")]
    public string? KeyvaultResourceId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identity")]
    public string? UserAssignedIdentity { get; set; }
}