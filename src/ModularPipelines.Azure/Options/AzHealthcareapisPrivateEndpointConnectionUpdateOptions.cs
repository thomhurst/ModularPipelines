using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "private-endpoint-connection", "update")]
public record AzHealthcareapisPrivateEndpointConnectionUpdateOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-link-service-connection-state")]
    public string? PrivateLinkServiceConnectionState { get; set; }

    [CommandSwitch("--private-link-service-connection-state-actions-required")]
    public string? PrivateLinkServiceConnectionStateActionsRequired { get; set; }

    [CommandSwitch("--private-link-service-connection-state-description")]
    public string? PrivateLinkServiceConnectionStateDescription { get; set; }

    [CommandSwitch("--private-link-service-connection-state-status")]
    public string? PrivateLinkServiceConnectionStateStatus { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}