using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "copy-snapshot")]
public record AwsLightsailCopySnapshotOptions(
[property: CommandSwitch("--target-snapshot-name")] string TargetSnapshotName,
[property: CommandSwitch("--source-region")] string SourceRegion
) : AwsOptions
{
    [CommandSwitch("--source-snapshot-name")]
    public string? SourceSnapshotName { get; set; }

    [CommandSwitch("--source-resource-name")]
    public string? SourceResourceName { get; set; }

    [CommandSwitch("--restore-date")]
    public string? RestoreDate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}