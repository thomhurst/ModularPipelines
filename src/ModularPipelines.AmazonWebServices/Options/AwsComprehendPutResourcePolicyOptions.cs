using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "put-resource-policy")]
public record AwsComprehendPutResourcePolicyOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CommandSwitch("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}