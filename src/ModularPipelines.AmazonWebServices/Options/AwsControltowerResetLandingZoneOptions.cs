using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "reset-landing-zone")]
public record AwsControltowerResetLandingZoneOptions(
[property: CliOption("--landing-zone-identifier")] string LandingZoneIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}