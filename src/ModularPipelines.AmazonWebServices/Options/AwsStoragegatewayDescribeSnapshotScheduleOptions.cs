using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-snapshot-schedule")]
public record AwsStoragegatewayDescribeSnapshotScheduleOptions(
[property: CommandSwitch("--volume-arn")] string VolumeArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}