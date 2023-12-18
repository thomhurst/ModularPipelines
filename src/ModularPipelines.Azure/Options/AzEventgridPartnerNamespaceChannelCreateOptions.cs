using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "channel", "create")]
public record AzEventgridPartnerNamespaceChannelCreateOptions(
[property: CommandSwitch("--channel-type")] string ChannelType,
[property: CommandSwitch("--destination-rg")] string DestinationRg,
[property: CommandSwitch("--destination-sub-id")] string DestinationSubId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-namespace-name")] string PartnerNamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-app-id")]
    public string? AadAppId { get; set; }

    [CommandSwitch("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CommandSwitch("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CommandSwitch("--ed-serv-cont")]
    public string? EdServCont { get; set; }

    [CommandSwitch("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [CommandSwitch("--event-type-kind")]
    public string? EventTypeKind { get; set; }

    [CommandSwitch("--inline-event-type")]
    public string? InlineEventType { get; set; }

    [CommandSwitch("--message-for-activation")]
    public string? MessageForActivation { get; set; }

    [CommandSwitch("--partner-destination-name")]
    public string? PartnerDestinationName { get; set; }

    [CommandSwitch("--partner-topic-name")]
    public string? PartnerTopicName { get; set; }

    [CommandSwitch("--partner-topic-source")]
    public string? PartnerTopicSource { get; set; }
}