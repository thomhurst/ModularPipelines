using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "describe-test-set-discrepancy-report")]
public record AwsLexv2ModelsDescribeTestSetDiscrepancyReportOptions(
[property: CommandSwitch("--test-set-discrepancy-report-id")] string TestSetDiscrepancyReportId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}