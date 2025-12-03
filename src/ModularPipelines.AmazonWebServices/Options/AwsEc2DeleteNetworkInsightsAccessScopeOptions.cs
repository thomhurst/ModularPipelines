using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-insights-access-scope")]
public record AwsEc2DeleteNetworkInsightsAccessScopeOptions(
[property: CliOption("--network-insights-access-scope-id")] string NetworkInsightsAccessScopeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}