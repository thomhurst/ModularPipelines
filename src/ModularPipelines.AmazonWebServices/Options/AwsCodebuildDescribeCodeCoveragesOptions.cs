using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "describe-code-coverages")]
public record AwsCodebuildDescribeCodeCoveragesOptions(
[property: CommandSwitch("--report-arn")] string ReportArn
) : AwsOptions
{
    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--min-line-coverage-percentage")]
    public double? MinLineCoveragePercentage { get; set; }

    [CommandSwitch("--max-line-coverage-percentage")]
    public double? MaxLineCoveragePercentage { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}