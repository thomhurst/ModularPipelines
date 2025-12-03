using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edgeorder", "order-item", "return")]
public record AzEdgeorderOrderItemReturnOptions(
[property: CliOption("--return-reason")] string ReturnReason
) : AzOptions
{
    [CliOption("--contact-details")]
    public string? ContactDetails { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-tag")]
    public string? ServiceTag { get; set; }

    [CliOption("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CliFlag("--shipping-box-required")]
    public bool? ShippingBoxRequired { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}