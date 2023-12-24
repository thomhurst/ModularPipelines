using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "delete-bucket-lifecycle-configuration")]
public record AwsS3controlDeleteBucketLifecycleConfigurationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bucket")] string Bucket
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}