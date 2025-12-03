using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "set-resource-access-for-bucket")]
public record AwsLightsailSetResourceAccessForBucketOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--access")] string Access
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}