using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("controltower", "reset-landing-zone")]
public record AwsControltowerResetLandingZoneOptions(
[property: CommandSwitch("--landing-zone-identifier")] string LandingZoneIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}