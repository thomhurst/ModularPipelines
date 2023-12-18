using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "package", "delete")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageDeleteOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--plan-id")] string PlanId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }
}

