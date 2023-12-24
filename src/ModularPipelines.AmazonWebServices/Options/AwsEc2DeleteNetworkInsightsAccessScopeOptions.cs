using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-insights-access-scope")]
public record AwsEc2DeleteNetworkInsightsAccessScopeOptions(
[property: CommandSwitch("--network-insights-access-scope-id")] string NetworkInsightsAccessScopeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}