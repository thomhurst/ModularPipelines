using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "list-sites")]
public record AwsOutpostsListSitesOptions : AwsOptions
{
    [CliOption("--operating-address-country-code-filter")]
    public string[]? OperatingAddressCountryCodeFilter { get; set; }

    [CliOption("--operating-address-state-or-region-filter")]
    public string[]? OperatingAddressStateOrRegionFilter { get; set; }

    [CliOption("--operating-address-city-filter")]
    public string[]? OperatingAddressCityFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}