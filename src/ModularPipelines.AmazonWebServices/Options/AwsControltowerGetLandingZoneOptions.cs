using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "get-landing-zone")]
public record AwsControltowerGetLandingZoneOptions(
[property: CliOption("--landing-zone-identifier")] string LandingZoneIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}