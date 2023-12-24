using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-target-group-attributes")]
public record AwsElbv2ModifyTargetGroupAttributesOptions(
[property: CommandSwitch("--target-group-arn")] string TargetGroupArn,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}