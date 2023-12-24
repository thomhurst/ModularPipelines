using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "delete-snapshot-schedule")]
public record AwsStoragegatewayDeleteSnapshotScheduleOptions(
[property: CommandSwitch("--volume-arn")] string VolumeArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}