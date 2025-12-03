using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-findings-feedback")]
public record AwsGuarddutyUpdateFindingsFeedbackOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--finding-ids")] string[] FindingIds,
[property: CliOption("--feedback")] string Feedback
) : AwsOptions
{
    [CliOption("--comments")]
    public string? Comments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}