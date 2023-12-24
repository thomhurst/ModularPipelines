using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-stored-iscsi-volumes")]
public record AwsStoragegatewayDescribeStoredIscsiVolumesOptions(
[property: CommandSwitch("--volume-arns")] string[] VolumeArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}