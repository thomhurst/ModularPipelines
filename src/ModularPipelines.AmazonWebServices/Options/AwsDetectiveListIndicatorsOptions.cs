using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "list-indicators")]
public record AwsDetectiveListIndicatorsOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--investigation-id")] string InvestigationId
) : AwsOptions
{
    [CommandSwitch("--indicator-type")]
    public string? IndicatorType { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}