using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-bucket-bundle")]
public record AwsLightsailUpdateBucketBundleOptions(
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--bundle-id")] string BundleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}