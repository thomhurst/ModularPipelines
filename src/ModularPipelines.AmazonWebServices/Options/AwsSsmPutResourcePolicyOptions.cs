using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "put-resource-policy")]
public record AwsSsmPutResourcePolicyOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--policy-hash")]
    public string? PolicyHash { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}