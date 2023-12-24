using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "delete-lifecycle-policy")]
public record AwsImagebuilderDeleteLifecyclePolicyOptions(
[property: CommandSwitch("--lifecycle-policy-arn")] string LifecyclePolicyArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}