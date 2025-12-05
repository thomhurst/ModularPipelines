using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "get-geo-location")]
public record AwsRoute53GetGeoLocationOptions : AwsOptions
{
    [CliOption("--continent-code")]
    public string? ContinentCode { get; set; }

    [CliOption("--country-code")]
    public string? CountryCode { get; set; }

    [CliOption("--subdivision-code")]
    public string? SubdivisionCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}