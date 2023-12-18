using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic", "event-subscription", "delete")]
public record AzEventgridPartnerTopicEventSubscriptionDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-topic-name")] string PartnerTopicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}