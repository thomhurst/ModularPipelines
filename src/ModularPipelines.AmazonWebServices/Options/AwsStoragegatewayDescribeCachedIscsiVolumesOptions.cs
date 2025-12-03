using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "describe-cached-iscsi-volumes")]
public record AwsStoragegatewayDescribeCachedIscsiVolumesOptions(
[property: CliOption("--volume-arns")] string[] VolumeArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}