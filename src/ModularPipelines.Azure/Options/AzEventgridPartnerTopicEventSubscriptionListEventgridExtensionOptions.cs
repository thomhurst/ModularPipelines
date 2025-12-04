using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "topic", "event-subscription", "list", "(eventgrid", "extension)")]
public record AzEventgridPartnerTopicEventSubscriptionListEventgridExtensionOptions(
[property: CliOption("--partner-topic-name")] string PartnerTopicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}