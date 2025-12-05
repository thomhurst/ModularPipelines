using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-target-group-attributes")]
public record AwsElbv2ModifyTargetGroupAttributesOptions(
[property: CliOption("--target-group-arn")] string TargetGroupArn,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}