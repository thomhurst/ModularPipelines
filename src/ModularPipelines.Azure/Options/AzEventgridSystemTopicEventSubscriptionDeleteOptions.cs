using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "event-subscription", "delete")]
public record AzEventgridSystemTopicEventSubscriptionDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}