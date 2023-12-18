using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "private-endpoint-connection", "create")]
public record AzHealthcareapisPrivateEndpointConnectionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
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
}