using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "create")]
public record AzPartnercenterMarketplaceOfferCreateOptions(
[property: CommandSwitch("--alias")] string Alias,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}