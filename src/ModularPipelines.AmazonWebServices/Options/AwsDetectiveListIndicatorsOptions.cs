using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "list-indicators")]
public record AwsDetectiveListIndicatorsOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--investigation-id")] string InvestigationId
) : AwsOptions
{
    [CliOption("--indicator-type")]
    public string? IndicatorType { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}