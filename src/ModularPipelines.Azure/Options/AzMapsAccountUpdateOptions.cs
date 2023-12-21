using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maps", "account", "update")]
public record AzMapsAccountUpdateOptions(
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--linked-resources")]
    public string? LinkedResources { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--user-identities")]
    public string? UserIdentities { get; set; }
}