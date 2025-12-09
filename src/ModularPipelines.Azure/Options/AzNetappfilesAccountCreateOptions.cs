using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "create")]
public record AzNetappfilesAccountCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-source")]
    public string? KeySource { get; set; }

    [CliOption("--key-vault-uri")]
    public string? KeyVaultUri { get; set; }

    [CliOption("--keyvault-resource-id")]
    public string? KeyvaultResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identity")]
    public string? UserAssignedIdentity { get; set; }
}