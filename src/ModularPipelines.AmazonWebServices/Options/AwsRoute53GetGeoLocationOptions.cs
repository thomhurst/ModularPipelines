using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "get-geo-location")]
public record AwsRoute53GetGeoLocationOptions : AwsOptions
{
    [CommandSwitch("--continent-code")]
    public string? ContinentCode { get; set; }

    [CommandSwitch("--country-code")]
    public string? CountryCode { get; set; }

    [CommandSwitch("--subdivision-code")]
    public string? SubdivisionCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}