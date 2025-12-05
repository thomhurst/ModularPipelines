using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("edgeorder", "order-item", "update")]
public record AzEdgeorderOrderItemUpdateOptions : AzOptions
{
    [CliOption("--contact-details")]
    public string? ContactDetails { get; set; }

    [CliOption("--encryption-preferences")]
    public string? EncryptionPreferences { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--mgmt-preferences")]
    public string? MgmtPreferences { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--notif-email-list")]
    public string? NotifEmailList { get; set; }

    [CliOption("--notif-preferences")]
    public string? NotifPreferences { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--transport-preferences")]
    public string? TransportPreferences { get; set; }
}