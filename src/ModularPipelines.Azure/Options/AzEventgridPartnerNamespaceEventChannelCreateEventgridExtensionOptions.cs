using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "event-channel", "create", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceEventChannelCreateEventgridExtensionOptions(
[property: CommandSwitch("--desination-topic-name")] string DesinationTopicName,
[property: CommandSwitch("--destination-resource-group")] string DestinationResourceGroup,
[property: CommandSwitch("--destination-subscription-id")] string DestinationSubscriptionId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-namespace-name")] string PartnerNamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--activation-expiration-date")]
    public string? ActivationExpirationDate { get; set; }

    [CommandSwitch("--partner-topic-description")]
    public string? PartnerTopicDescription { get; set; }

    [CommandSwitch("--publisher-filter")]
    public string? PublisherFilter { get; set; }
}