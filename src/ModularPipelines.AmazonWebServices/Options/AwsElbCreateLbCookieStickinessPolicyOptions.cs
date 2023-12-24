using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "create-lb-cookie-stickiness-policy")]
public record AwsElbCreateLbCookieStickinessPolicyOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--cookie-expiration-period")]
    public long? CookieExpirationPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}