using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "topic", "event-subscription", "delete")]
public record AzEventgridDomainTopicEventSubscriptionDeleteOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--domain-topic-name")] string DomainTopicName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}