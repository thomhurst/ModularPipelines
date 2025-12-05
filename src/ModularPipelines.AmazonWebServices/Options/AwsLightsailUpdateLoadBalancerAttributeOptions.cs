using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-load-balancer-attribute")]
public record AwsLightsailUpdateLoadBalancerAttributeOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--attribute-name")] string AttributeName,
[property: CliOption("--attribute-value")] string AttributeValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}