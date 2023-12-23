using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "update-traffic-policy-instance")]
public record AwsRoute53UpdateTrafficPolicyInstanceOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--ttl")] long Ttl,
[property: CommandSwitch("--traffic-policy-id")] string TrafficPolicyId,
[property: CommandSwitch("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}