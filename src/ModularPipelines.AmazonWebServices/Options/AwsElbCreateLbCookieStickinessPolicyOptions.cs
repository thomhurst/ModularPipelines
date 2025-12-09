using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "create-lb-cookie-stickiness-policy")]
public record AwsElbCreateLbCookieStickinessPolicyOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--cookie-expiration-period")]
    public long? CookieExpirationPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}