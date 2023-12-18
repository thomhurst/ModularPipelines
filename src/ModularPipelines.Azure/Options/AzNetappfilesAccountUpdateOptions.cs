using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "update")]
public record AzNetappfilesAccountUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--key-source")]
    public string? KeySource { get; set; }

    [CommandSwitch("--key-vault-uri")]
    public string? KeyVaultUri { get; set; }

    [CommandSwitch("--keyvault-resource-id")]
    public string? KeyvaultResourceId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identity")]
    public string? UserAssignedIdentity { get; set; }
}