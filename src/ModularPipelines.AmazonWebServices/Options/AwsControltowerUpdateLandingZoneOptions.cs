using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("controltower", "update-landing-zone")]
public record AwsControltowerUpdateLandingZoneOptions(
[property: CommandSwitch("--landing-zone-identifier")] string LandingZoneIdentifier,
[property: CommandSwitch("--manifest")] string Manifest,
[property: CommandSwitch("--landing-zone-version")] string LandingZoneVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}