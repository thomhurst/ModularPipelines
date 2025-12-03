using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-traffic-distribution")]
public record AwsConnectUpdateTrafficDistributionOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--telephony-config")]
    public string? TelephonyConfig { get; set; }

    [CliOption("--sign-in-config")]
    public string? SignInConfig { get; set; }

    [CliOption("--agent-config")]
    public string? AgentConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}