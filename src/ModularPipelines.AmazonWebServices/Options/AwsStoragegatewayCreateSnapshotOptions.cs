using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-snapshot")]
public record AwsStoragegatewayCreateSnapshotOptions(
[property: CommandSwitch("--volume-arn")] string VolumeArn,
[property: CommandSwitch("--snapshot-description")] string SnapshotDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}