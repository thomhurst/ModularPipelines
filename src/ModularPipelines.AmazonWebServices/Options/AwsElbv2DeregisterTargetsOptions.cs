using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "deregister-targets")]
public record AwsElbv2DeregisterTargetsOptions(
[property: CliOption("--target-group-arn")] string TargetGroupArn,
[property: CliOption("--targets")] string[] Targets
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}