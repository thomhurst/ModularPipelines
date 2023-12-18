using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "order-item", "return")]
public record AzEdgeorderOrderItemReturnOptions(
[property: CommandSwitch("--return-reason")] string ReturnReason
) : AzOptions
{
    [CommandSwitch("--contact-details")]
    public string? ContactDetails { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-tag")]
    public string? ServiceTag { get; set; }

    [CommandSwitch("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [BooleanCommandSwitch("--shipping-box-required")]
    public bool? ShippingBoxRequired { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}