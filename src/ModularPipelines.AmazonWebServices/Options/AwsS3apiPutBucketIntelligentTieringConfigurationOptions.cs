using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-bucket-intelligent-tiering-configuration")]
public record AwsS3apiPutBucketIntelligentTieringConfigurationOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--intelligent-tiering-configuration")] string IntelligentTieringConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}