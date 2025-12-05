using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-geo-locations")]
public record AwsRoute53ListGeoLocationsOptions : AwsOptions
{
    [CliOption("--start-continent-code")]
    public string? StartContinentCode { get; set; }

    [CliOption("--start-country-code")]
    public string? StartCountryCode { get; set; }

    [CliOption("--start-subdivision-code")]
    public string? StartSubdivisionCode { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}