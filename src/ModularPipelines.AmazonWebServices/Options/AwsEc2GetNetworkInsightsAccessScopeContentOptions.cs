using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-network-insights-access-scope-content")]
public record AwsEc2GetNetworkInsightsAccessScopeContentOptions(
[property: CliOption("--network-insights-access-scope-id")] string NetworkInsightsAccessScopeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}