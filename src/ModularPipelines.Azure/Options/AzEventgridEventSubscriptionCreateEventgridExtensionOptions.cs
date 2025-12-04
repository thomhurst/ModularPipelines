using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "event-subscription", "create", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionCreateEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CliOption("--azure-active-directory-application-id-or-uri")]
    public string? AzureActiveDirectoryApplicationIdOrUri { get; set; }

    [CliOption("--azure-active-directory-tenant-id")]
    public string? AzureActiveDirectoryTenantId { get; set; }

    [CliOption("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CliOption("--deadletter-identity")]
    public string? DeadletterIdentity { get; set; }

    [CliOption("--deadletter-identity-endpoint")]
    public string? DeadletterIdentityEndpoint { get; set; }

    [CliOption("--delivery-identity")]
    public string? DeliveryIdentity { get; set; }

    [CliOption("--delivery-identity-endpoint")]
    public string? DeliveryIdentityEndpoint { get; set; }

    [CliOption("--delivery-identity-endpoint-type")]
    public string? DeliveryIdentityEndpointType { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CliOption("--event-ttl")]
    public string? EventTtl { get; set; }

    [CliOption("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CliOption("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--max-delivery-attempts")]
    public string? MaxDeliveryAttempts { get; set; }

    [CliOption("--max-events-per-batch")]
    public string? MaxEventsPerBatch { get; set; }

    [CliOption("--preferred-batch-size-in-kilobytes")]
    public string? PreferredBatchSizeInKilobytes { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CliOption("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CliFlag("--subject-case-sensitive")]
    public bool? SubjectCaseSensitive { get; set; }

    [CliOption("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}