using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-continuous-deployment-policy")]
public record AwsCloudfrontUpdateContinuousDeploymentPolicyOptions(
[property: CliOption("--continuous-deployment-policy-config")] string ContinuousDeploymentPolicyConfig,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}