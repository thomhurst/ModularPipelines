using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-threat-intel-set")]
public record AwsGuarddutyUpdateThreatIntelSetOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--threat-intel-set-id")] string ThreatIntelSetId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}