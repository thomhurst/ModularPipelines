using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "order-item", "update")]
public record AzEdgeorderOrderItemUpdateOptions : AzOptions
{
    [CommandSwitch("--contact-details")]
    public string? ContactDetails { get; set; }

    [CommandSwitch("--encryption-preferences")]
    public string? EncryptionPreferences { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--mgmt-preferences")]
    public string? MgmtPreferences { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--notif-email-list")]
    public string? NotifEmailList { get; set; }

    [CommandSwitch("--notif-preferences")]
    public string? NotifPreferences { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--transport-preferences")]
    public string? TransportPreferences { get; set; }
}