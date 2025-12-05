using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "package", "add")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageAddOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--plan-id")] string PlanId
) : AzOptions
{
    [CliOption("--cluster-extension-type")]
    public string? ClusterExtensionType { get; set; }

    [CliOption("--digest")]
    public string? Digest { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--registry")]
    public string? Registry { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}