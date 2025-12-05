using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-analysis")]
public record AwsQuicksightDescribeAnalysisOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--analysis-id")] string AnalysisId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}