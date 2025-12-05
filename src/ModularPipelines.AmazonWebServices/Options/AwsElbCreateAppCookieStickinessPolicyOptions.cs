using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "create-app-cookie-stickiness-policy")]
public record AwsElbCreateAppCookieStickinessPolicyOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--cookie-name")] string CookieName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}