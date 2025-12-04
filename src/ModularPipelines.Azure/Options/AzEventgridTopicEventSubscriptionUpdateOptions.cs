using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "topic", "event-subscription", "update")]
public record AzEventgridTopicEventSubscriptionUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CliOption("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CliOption("--delivery-attribute-mapping")]
    public string? DeliveryAttributeMapping { get; set; }

    [CliFlag("--enable-advanced-filtering-on-arrays")]
    public bool? EnableAdvancedFilteringOnArrays { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--qttl")]
    public string? Qttl { get; set; }

    [CliOption("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CliOption("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }

    [CliOption("--update-endpoint-type")]
    public string? UpdateEndpointType { get; set; }
}