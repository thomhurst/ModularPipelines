using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic", "event-subscription", "list")]
public record AzEventgridTopicEventSubscriptionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}