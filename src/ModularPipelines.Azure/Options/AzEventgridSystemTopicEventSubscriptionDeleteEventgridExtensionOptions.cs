using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription", "delete", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicEventSubscriptionDeleteEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--system-topic-name")] string SystemTopicName
) : AzOptions;