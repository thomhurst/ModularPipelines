using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maps", "account", "create")]
public record AzMapsAccountCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--accept-tos")]
    public string? AcceptTos { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--linked-resources")]
    public string? LinkedResources { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--user-identities")]
    public string? UserIdentities { get; set; }
}

