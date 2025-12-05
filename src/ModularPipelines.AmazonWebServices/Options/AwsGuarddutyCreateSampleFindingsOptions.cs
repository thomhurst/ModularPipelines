using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "create-sample-findings")]
public record AwsGuarddutyCreateSampleFindingsOptions(
[property: CliOption("--detector-id")] string DetectorId
) : AwsOptions
{
    [CliOption("--finding-types")]
    public string[]? FindingTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}