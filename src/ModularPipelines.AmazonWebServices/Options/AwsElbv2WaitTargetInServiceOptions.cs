using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "wait", "target-in-service")]
public record AwsElbv2WaitTargetInServiceOptions(
[property: CommandSwitch("--target-group-arn")] string TargetGroupArn
) : AwsOptions
{
    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--include")]
    public string[]? Include { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}