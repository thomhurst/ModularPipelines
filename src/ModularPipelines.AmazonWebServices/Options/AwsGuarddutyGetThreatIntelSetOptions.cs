using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-threat-intel-set")]
public record AwsGuarddutyGetThreatIntelSetOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--threat-intel-set-id")] string ThreatIntelSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}