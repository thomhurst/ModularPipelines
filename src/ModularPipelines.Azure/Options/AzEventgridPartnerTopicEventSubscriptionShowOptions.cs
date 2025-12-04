using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "topic", "event-subscription", "show")]
public record AzEventgridPartnerTopicEventSubscriptionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-topic-name")] string PartnerTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }

    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }
}