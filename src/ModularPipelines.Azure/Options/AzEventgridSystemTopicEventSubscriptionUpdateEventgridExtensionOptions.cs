using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "event-subscription", "update", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicEventSubscriptionUpdateEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CliOption("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CliOption("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CliOption("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}