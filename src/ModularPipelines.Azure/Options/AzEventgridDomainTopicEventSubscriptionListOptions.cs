using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "topic", "event-subscription", "list")]
public record AzEventgridDomainTopicEventSubscriptionListOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--domain-topic-name")] string DomainTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}