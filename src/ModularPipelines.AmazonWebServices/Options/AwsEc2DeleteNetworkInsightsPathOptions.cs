using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-insights-path")]
public record AwsEc2DeleteNetworkInsightsPathOptions(
[property: CommandSwitch("--network-insights-path-id")] string NetworkInsightsPathId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}