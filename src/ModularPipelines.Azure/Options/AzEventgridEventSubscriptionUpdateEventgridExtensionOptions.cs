using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "update", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionUpdateEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CommandSwitch("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CommandSwitch("--deadletter-identity")]
    public string? DeadletterIdentity { get; set; }

    [CommandSwitch("--deadletter-identity-endpoint")]
    public string? DeadletterIdentityEndpoint { get; set; }

    [CommandSwitch("--delivery-identity")]
    public string? DeliveryIdentity { get; set; }

    [CommandSwitch("--delivery-identity-endpoint")]
    public string? DeliveryIdentityEndpoint { get; set; }

    [CommandSwitch("--delivery-identity-endpoint-type")]
    public string? DeliveryIdentityEndpointType { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CommandSwitch("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CommandSwitch("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}