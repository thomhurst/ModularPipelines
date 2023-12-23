using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "list-sites")]
public record AwsOutpostsListSitesOptions : AwsOptions
{
    [CommandSwitch("--operating-address-country-code-filter")]
    public string[]? OperatingAddressCountryCodeFilter { get; set; }

    [CommandSwitch("--operating-address-state-or-region-filter")]
    public string[]? OperatingAddressStateOrRegionFilter { get; set; }

    [CommandSwitch("--operating-address-city-filter")]
    public string[]? OperatingAddressCityFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}