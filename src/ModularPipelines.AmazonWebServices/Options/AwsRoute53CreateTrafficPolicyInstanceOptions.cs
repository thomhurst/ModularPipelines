using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-traffic-policy-instance")]
public record AwsRoute53CreateTrafficPolicyInstanceOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--ttl")] long Ttl,
[property: CommandSwitch("--traffic-policy-id")] string TrafficPolicyId,
[property: CommandSwitch("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}