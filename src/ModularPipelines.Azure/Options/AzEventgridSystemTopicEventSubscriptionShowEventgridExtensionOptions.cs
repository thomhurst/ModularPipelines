using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicEventSubscriptionShowEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}