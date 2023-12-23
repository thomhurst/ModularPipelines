using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-continuous-deployment-policy")]
public record AwsCloudfrontUpdateContinuousDeploymentPolicyOptions(
[property: CommandSwitch("--continuous-deployment-policy-config")] string ContinuousDeploymentPolicyConfig,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}