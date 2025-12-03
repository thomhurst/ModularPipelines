using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "channel", "update")]
public record AzEventgridPartnerNamespaceChannelUpdateOptions : AzOptions
{
    [CliOption("--aad-app-id")]
    public string? AadAppId { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--act-exp-date")]
    public string? ActExpDate { get; set; }

    [CliOption("--endpoint-base-url")]
    public string? EndpointBaseUrl { get; set; }

    [CliOption("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [CliOption("--event-type-kind")]
    public string? EventTypeKind { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--inline-event-type")]
    public string? InlineEventType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--partner-namespace-name")]
    public string? PartnerNamespaceName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}