using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-bucket-access-key")]
public record AwsLightsailDeleteBucketAccessKeyOptions(
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--access-key-id")] string AccessKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}