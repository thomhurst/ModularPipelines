using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-test-set-discrepancy-report")]
public record AwsLexv2ModelsCreateTestSetDiscrepancyReportOptions(
[property: CliOption("--test-set-id")] string TestSetId,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}