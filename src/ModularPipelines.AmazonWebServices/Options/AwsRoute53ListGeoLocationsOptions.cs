using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-geo-locations")]
public record AwsRoute53ListGeoLocationsOptions : AwsOptions
{
    [CommandSwitch("--start-continent-code")]
    public string? StartContinentCode { get; set; }

    [CommandSwitch("--start-country-code")]
    public string? StartCountryCode { get; set; }

    [CommandSwitch("--start-subdivision-code")]
    public string? StartSubdivisionCode { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}