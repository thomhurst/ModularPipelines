using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-continuous-deployment-policy")]
public record AwsCloudfrontCreateContinuousDeploymentPolicyOptions(
[property: CliOption("--continuous-deployment-policy-config")] string ContinuousDeploymentPolicyConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}