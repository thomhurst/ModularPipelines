using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "update-landing-zone")]
public record AwsControltowerUpdateLandingZoneOptions(
[property: CliOption("--landing-zone-identifier")] string LandingZoneIdentifier,
[property: CliOption("--manifest")] string Manifest,
[property: CliOption("--landing-zone-version")] string LandingZoneVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}