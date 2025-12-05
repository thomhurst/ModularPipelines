using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "setup", "update")]
public record AzPartnercenterMarketplaceOfferSetupUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliFlag("--reseller")]
    public bool? Reseller { get; set; }

    [CliFlag("--sell-through-microsoft")]
    public bool? SellThroughMicrosoft { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--test-drive")]
    public bool? TestDrive { get; set; }

    [CliOption("--trial-uri")]
    public string? TrialUri { get; set; }
}