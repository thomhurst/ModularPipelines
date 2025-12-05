using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "describe-stored-iscsi-volumes")]
public record AwsStoragegatewayDescribeStoredIscsiVolumesOptions(
[property: CliOption("--volume-arns")] string[] VolumeArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}