using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "restore-volume-from-snapshot")]
public record AwsFsxRestoreVolumeFromSnapshotOptions(
[property: CommandSwitch("--volume-id")] string VolumeId,
[property: CommandSwitch("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--options")]
    public string[]? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}