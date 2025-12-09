using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "start-network-insights-analysis")]
public record AwsEc2StartNetworkInsightsAnalysisOptions(
[property: CliOption("--network-insights-path-id")] string NetworkInsightsPathId
) : AwsOptions
{
    [CliOption("--additional-accounts")]
    public string[]? AdditionalAccounts { get; set; }

    [CliOption("--filter-in-arns")]
    public string[]? FilterInArns { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}