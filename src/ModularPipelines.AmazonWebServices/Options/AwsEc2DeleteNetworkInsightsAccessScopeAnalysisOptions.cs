using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-insights-access-scope-analysis")]
public record AwsEc2DeleteNetworkInsightsAccessScopeAnalysisOptions(
[property: CommandSwitch("--network-insights-access-scope-analysis-id")] string NetworkInsightsAccessScopeAnalysisId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}