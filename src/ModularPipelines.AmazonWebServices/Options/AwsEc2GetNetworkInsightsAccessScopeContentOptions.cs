using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-network-insights-access-scope-content")]
public record AwsEc2GetNetworkInsightsAccessScopeContentOptions(
[property: CommandSwitch("--network-insights-access-scope-id")] string NetworkInsightsAccessScopeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}