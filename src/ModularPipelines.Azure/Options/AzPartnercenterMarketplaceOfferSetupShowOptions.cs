using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "setup", "show")]
public record AzPartnercenterMarketplaceOfferSetupShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [BooleanCommandSwitch("--reseller")]
    public bool? Reseller { get; set; }

    [BooleanCommandSwitch("--sell-through-microsoft")]
    public bool? SellThroughMicrosoft { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--test-drive")]
    public bool? TestDrive { get; set; }

    [CommandSwitch("--trial-uri")]
    public string? TrialUri { get; set; }
}