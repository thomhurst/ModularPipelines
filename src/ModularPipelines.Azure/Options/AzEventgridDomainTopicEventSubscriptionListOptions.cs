using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain", "topic", "event-subscription", "list")]
public record AzEventgridDomainTopicEventSubscriptionListOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--domain-topic-name")] string DomainTopicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }
}