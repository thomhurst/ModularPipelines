using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "package", "delete")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageDeleteOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--plan-id")] string PlanId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }
}