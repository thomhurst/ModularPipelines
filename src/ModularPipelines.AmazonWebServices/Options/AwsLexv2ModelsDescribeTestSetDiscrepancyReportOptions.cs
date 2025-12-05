using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-test-set-discrepancy-report")]
public record AwsLexv2ModelsDescribeTestSetDiscrepancyReportOptions(
[property: CliOption("--test-set-discrepancy-report-id")] string TestSetDiscrepancyReportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}