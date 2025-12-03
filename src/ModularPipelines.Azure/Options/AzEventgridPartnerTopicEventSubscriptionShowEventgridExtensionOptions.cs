using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridPartnerTopicEventSubscriptionShowEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-topic-name")] string PartnerTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}