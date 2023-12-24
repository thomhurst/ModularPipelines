using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-cached-iscsi-volumes")]
public record AwsStoragegatewayDescribeCachedIscsiVolumesOptions(
[property: CommandSwitch("--volume-arns")] string[] VolumeArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}