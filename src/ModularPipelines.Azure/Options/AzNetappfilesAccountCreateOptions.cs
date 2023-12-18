using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "create")]
public record AzNetappfilesAccountCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--key-source")]
    public string? KeySource { get; set; }

    [CommandSwitch("--key-vault-uri")]
    public string? KeyVaultUri { get; set; }

    [CommandSwitch("--keyvault-resource-id")]
    public string? KeyvaultResourceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identity")]
    public string? UserAssignedIdentity { get; set; }
}