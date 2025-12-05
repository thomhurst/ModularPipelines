using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "put-resource-policy")]
public record AwsCloudtrailPutResourcePolicyOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}