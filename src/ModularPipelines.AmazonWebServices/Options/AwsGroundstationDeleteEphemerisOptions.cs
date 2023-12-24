using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "delete-ephemeris")]
public record AwsGroundstationDeleteEphemerisOptions(
[property: CommandSwitch("--ephemeris-id")] string EphemerisId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}