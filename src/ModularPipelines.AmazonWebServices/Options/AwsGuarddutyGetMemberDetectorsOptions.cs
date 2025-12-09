using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-member-detectors")]
public record AwsGuarddutyGetMemberDetectorsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}