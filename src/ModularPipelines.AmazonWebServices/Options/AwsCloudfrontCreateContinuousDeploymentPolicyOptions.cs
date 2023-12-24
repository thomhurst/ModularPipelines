using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-continuous-deployment-policy")]
public record AwsCloudfrontCreateContinuousDeploymentPolicyOptions(
[property: CommandSwitch("--continuous-deployment-policy-config")] string ContinuousDeploymentPolicyConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}