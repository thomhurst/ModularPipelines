using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-traffic-distribution")]
public record AwsConnectUpdateTrafficDistributionOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--telephony-config")]
    public string? TelephonyConfig { get; set; }

    [CommandSwitch("--sign-in-config")]
    public string? SignInConfig { get; set; }

    [CommandSwitch("--agent-config")]
    public string? AgentConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}