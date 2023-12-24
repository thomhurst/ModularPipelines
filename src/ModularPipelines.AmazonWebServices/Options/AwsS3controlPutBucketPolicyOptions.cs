using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-bucket-policy")]
public record AwsS3controlPutBucketPolicyOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}