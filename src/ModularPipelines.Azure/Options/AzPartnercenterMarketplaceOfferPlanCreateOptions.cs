using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "create")]
public record AzPartnercenterMarketplaceOfferPlanCreateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subtype")]
    public string? Subtype { get; set; }
}