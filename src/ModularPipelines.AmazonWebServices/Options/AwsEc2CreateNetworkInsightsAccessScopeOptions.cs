using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-network-insights-access-scope")]
public record AwsEc2CreateNetworkInsightsAccessScopeOptions : AwsOptions
{
    [CliOption("--match-paths")]
    public string[]? MatchPaths { get; set; }

    [CliOption("--exclude-paths")]
    public string[]? ExcludePaths { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}