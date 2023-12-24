using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "start-network-insights-analysis")]
public record AwsEc2StartNetworkInsightsAnalysisOptions(
[property: CommandSwitch("--network-insights-path-id")] string NetworkInsightsPathId
) : AwsOptions
{
    [CommandSwitch("--additional-accounts")]
    public string[]? AdditionalAccounts { get; set; }

    [CommandSwitch("--filter-in-arns")]
    public string[]? FilterInArns { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}