using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridPartnerTopicEventSubscriptionShowEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-topic-name")] string PartnerTopicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}