using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "put-resource-policy")]
public record AwsComprehendPutResourcePolicyOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CliOption("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}