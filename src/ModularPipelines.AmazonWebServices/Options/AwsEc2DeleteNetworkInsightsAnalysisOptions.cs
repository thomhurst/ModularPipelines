using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-insights-analysis")]
public record AwsEc2DeleteNetworkInsightsAnalysisOptions(
[property: CommandSwitch("--network-insights-analysis-id")] string NetworkInsightsAnalysisId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}