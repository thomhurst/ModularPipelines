using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "event-subscription", "list")]
public record AzEventgridSystemTopicEventSubscriptionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}