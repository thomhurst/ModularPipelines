using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "describe-code-coverages")]
public record AwsCodebuildDescribeCodeCoveragesOptions(
[property: CliOption("--report-arn")] string ReportArn
) : AwsOptions
{
    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--min-line-coverage-percentage")]
    public double? MinLineCoveragePercentage { get; set; }

    [CliOption("--max-line-coverage-percentage")]
    public double? MaxLineCoveragePercentage { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}