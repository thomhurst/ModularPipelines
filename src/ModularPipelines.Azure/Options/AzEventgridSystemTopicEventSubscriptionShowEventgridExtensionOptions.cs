using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicEventSubscriptionShowEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [BooleanCommandSwitch("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}