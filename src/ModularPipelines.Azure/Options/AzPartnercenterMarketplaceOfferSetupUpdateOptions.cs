using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "setup", "update")]
public record AzPartnercenterMarketplaceOfferSetupUpdateOptions(
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

