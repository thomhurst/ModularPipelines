using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "topic", "event-subscription", "create")]
public record AzEventgridPartnerTopicEventSubscriptionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-topic-name")] string PartnerTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CliOption("--azure-active-directory-application-id-or-uri")]
    public string? AzureActiveDirectoryApplicationIdOrUri { get; set; }

    [CliOption("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CliOption("--delivery-attribute-mapping")]
    public string? DeliveryAttributeMapping { get; set; }

    [CliFlag("--enable-advanced-filtering-on-arrays")]
    public bool? EnableAdvancedFilteringOnArrays { get; set; }

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

    [CliOption("--qttl")]
    public string? Qttl { get; set; }

    [CliOption("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CliFlag("--subject-case-sensitive")]
    public bool? SubjectCaseSensitive { get; set; }

    [CliOption("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}