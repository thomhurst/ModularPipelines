using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "describe-application-snapshot")]
public record AwsKinesisanalyticsv2DescribeApplicationSnapshotOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}