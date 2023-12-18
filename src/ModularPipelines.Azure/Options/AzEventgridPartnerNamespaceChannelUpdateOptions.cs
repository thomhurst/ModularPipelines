using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "channel", "update")]
public record AzEventgridPartnerNamespaceChannelUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-app-id")]
    public string? AadAppId { get; set; }

    [CommandSwitch("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CommandSwitch("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CommandSwitch("--endpoint-base-url")]
    public string? EndpointBaseUrl { get; set; }

    [CommandSwitch("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [CommandSwitch("--event-type-kind")]
    public string? EventTypeKind { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--inline-event-type")]
    public string? InlineEventType { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--partner-namespace-name")]
    public string? PartnerNamespaceName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}