using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "package", "add")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageAddOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--plan-id")] string PlanId
) : AzOptions
{
    [CommandSwitch("--cluster-extension-type")]
    public string? ClusterExtensionType { get; set; }

    [CommandSwitch("--digest")]
    public string? Digest { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--registry")]
    public string? Registry { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }
}