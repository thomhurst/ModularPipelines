using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "channel", "create")]
public record AzEventgridPartnerNamespaceChannelCreateOptions(
[property: CliOption("--channel-type")] string ChannelType,
[property: CliOption("--destination-rg")] string DestinationRg,
[property: CliOption("--destination-sub-id")] string DestinationSubId,
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-namespace-name")] string PartnerNamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-app-id")]
    public string? AadAppId { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CliOption("--ed-serv-cont")]
    public string? EdServCont { get; set; }

    [CliOption("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [CliOption("--event-type-kind")]
    public string? EventTypeKind { get; set; }

    [CliOption("--inline-event-type")]
    public string? InlineEventType { get; set; }

    [CliOption("--message-for-activation")]
    public string? MessageForActivation { get; set; }

    [CliOption("--partner-destination-name")]
    public string? PartnerDestinationName { get; set; }

    [CliOption("--partner-topic-name")]
    public string? PartnerTopicName { get; set; }

    [CliOption("--partner-topic-source")]
    public string? PartnerTopicSource { get; set; }
}