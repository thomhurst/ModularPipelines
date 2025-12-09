using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "wait", "target-deregistered")]
public record AwsElbv2WaitTargetDeregisteredOptions(
[property: CliOption("--target-group-arn")] string TargetGroupArn
) : AwsOptions
{
    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--include")]
    public string[]? Include { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}