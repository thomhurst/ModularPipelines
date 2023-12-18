using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "create", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionCreateEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CommandSwitch("--azure-active-directory-application-id-or-uri")]
    public string? AzureActiveDirectoryApplicationIdOrUri { get; set; }

    [CommandSwitch("--azure-active-directory-tenant-id")]
    public string? AzureActiveDirectoryTenantId { get; set; }

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

    [CommandSwitch("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CommandSwitch("--event-ttl")]
    public string? EventTtl { get; set; }

    [CommandSwitch("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CommandSwitch("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--max-delivery-attempts")]
    public string? MaxDeliveryAttempts { get; set; }

    [CommandSwitch("--max-events-per-batch")]
    public string? MaxEventsPerBatch { get; set; }

    [CommandSwitch("--preferred-batch-size-in-kilobytes")]
    public string? PreferredBatchSizeInKilobytes { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CommandSwitch("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [BooleanCommandSwitch("--subject-case-sensitive")]
    public bool? SubjectCaseSensitive { get; set; }

    [CommandSwitch("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}