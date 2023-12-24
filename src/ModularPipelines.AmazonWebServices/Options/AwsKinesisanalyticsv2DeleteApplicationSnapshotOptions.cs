using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "delete-application-snapshot")]
public record AwsKinesisanalyticsv2DeleteApplicationSnapshotOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--snapshot-name")] string SnapshotName,
[property: CommandSwitch("--snapshot-creation-timestamp")] long SnapshotCreationTimestamp
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}