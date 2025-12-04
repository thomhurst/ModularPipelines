using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "event-subscription", "show")]
public record AzEventgridSystemTopicEventSubscriptionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }

    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}