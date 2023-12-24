using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-network-insights-access-scope")]
public record AwsEc2CreateNetworkInsightsAccessScopeOptions : AwsOptions
{
    [CommandSwitch("--match-paths")]
    public string[]? MatchPaths { get; set; }

    [CommandSwitch("--exclude-paths")]
    public string[]? ExcludePaths { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}