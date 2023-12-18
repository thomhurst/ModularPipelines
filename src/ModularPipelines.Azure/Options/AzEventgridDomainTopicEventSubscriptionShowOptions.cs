using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain", "topic", "event-subscription", "show")]
public record AzEventgridDomainTopicEventSubscriptionShowOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--domain-topic-name")] string DomainTopicName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--full-ed-url")]
    public bool? FullEdUrl { get; set; }

    [BooleanCommandSwitch("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }
}

