using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-access-point-policy")]
public record AwsS3controlPutAccessPointPolicyOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}