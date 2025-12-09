using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "namespace", "event-channel", "create", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceEventChannelCreateEventgridExtensionOptions(
[property: CliOption("--desination-topic-name")] string DesinationTopicName,
[property: CliOption("--destination-resource-group")] string DestinationResourceGroup,
[property: CliOption("--destination-subscription-id")] string DestinationSubscriptionId,
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-namespace-name")] string PartnerNamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--activation-expiration-date")]
    public string? ActivationExpirationDate { get; set; }

    [CliOption("--partner-topic-description")]
    public string? PartnerTopicDescription { get; set; }

    [CliOption("--publisher-filter")]
    public string? PublisherFilter { get; set; }
}