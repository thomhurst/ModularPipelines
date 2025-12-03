using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic", "event-subscription", "delete", "(eventgrid", "extension)")]
public record AzEventgridPartnerTopicEventSubscriptionDeleteEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-topic-name")] string PartnerTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;